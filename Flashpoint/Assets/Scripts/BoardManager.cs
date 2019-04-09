using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

/**
 * Developer: Alex
 * Purpose: Manager for the board and all its spaces/pieces and work with GameManager
 **/


public class BoardManager : MonoBehaviour
{
    private int SavedWinThreshold = 5;
    public int HouseHP { get; private set; } //House Damage Loss Condition
    public int RemainingPOI { get; private set; } //POI Death Loss Condition
    public int SavedPOI { get; private set; } //POI Save Win condition

    public void HouseDamage() { HouseHP--; }
    public void PoiDeath() { RemainingPOI--; }
    public void PoiSaved() { SavedPOI++; }


    static String inFloorTag = "InsideTile";
    static String outFloorTag = "OutsideTile";
    static String wallTag = "Wall";
    static String inDoorTag = "DoorInside";
    static String outDoorTag = "DoorOutside";
    static String POItag = "POI";
    static String firemanTag = "Fireman";

    public static BoardManager Instance = null;
    public int columns = 10;
    public int rows = 8;

    public double tileSize = 4; // 4x4 size of every grid

    public Vector3 houseCorner = new Vector3(-16f, 0f, 16f);

    GameObject[,] floors;
    GameObject[,] leftEdge;
    GameObject[,] upperEdge;
    GameObject[,] ambulances;
    GameObject[,] deckguns;
    ArrayList[,] firemen;
    ArrayList[,] pois;

    /*
	 * Waiting on Features:
	 *	1 - Firefighter knockout
	 *	2 - POI death
	 *	3 - Server End Game
	 * */

    //-----------------------------+
    // PUBLIC API				   | : 
    //-----------------------------+

    //Add a space into the BoardManager. Because spaces are stationary, we can also store the space's coordinate in its own object
    void AddSpace(GameObject space, int x, int y)
    {
        if (!IsOnBoard(x, y))
        {
            throw new InvalidPositionException();
        }
        floors[x, y] = space;
        space.GetComponent<Space>().x = x;
        space.GetComponent<Space>().y = y;
    }
    void AddEdgeObstacle(GameObject edge, int x, int y, String direction)
    {
        if (!IsOnBoard(x, y))
        {
            throw new InvalidPositionException();
        }

        direction.ToLowerInvariant();

        if (direction.Equals("left"))
        {
            leftEdge[x, y] = edge;
        }
        else if (direction.Equals("up"))
        {
            leftEdge[x, y] = edge;
        }
        else
        {
            throw new ArgumentException("Direction must be either left or up.");
        }

    }

    //Return a reference to the GameObject of a space (launch pad) for coordinates x, y
    public GameObject GetSpace(int x, int y)
    {
        int[] c = { x, y };
        if (!IsOnBoard(c))
        {
            throw new InvalidPositionException();
        }

        return floors[x, y];
    }
    public GameObject GetSpace(GameObject spaceObject)
    {
        foreach (GameObject s in floors)
        {
            if (s == spaceObject) return s;
        }

        return null;
    }

    //Return the coordinates of a Launch Pad GameObject
    public int[] GetSpaceCoordinates(GameObject space)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (floors[x, y] == space)
                {
                    return new int[] { x, y };
                }
            }
        }

        //Not found
        return null;
    }

    //Return a reference to the GameObject of a door/wall for coordinates x, y
    public GameObject GetEdgeObstacle(int x, int y, String direction)
    {
        direction = direction.ToLowerInvariant();

        int[] c = { x, y };
        if (!IsOnBoard(c))
        {
            return null;
        }

        // Return either the edge above or to the left of the space
        if (direction.Equals("up"))
        {
            return upperEdge[x, y];
        }
        else if (direction.Equals("left"))
        {
            return leftEdge[x, y];
        }
        else if (direction.Equals("down"))
        {
            if (!IsOnBoard(new int[] { x, y + 1 }))
            {
                return null;
            }
            return upperEdge[x, y + 1];
            ;
        }
        else if (direction.Equals("right"))
        {
            if (!IsOnBoard(new int[] { x + 1, y }))
            {
                return null;
            }

            return leftEdge[x + 1, y];
        }
        else
        {
            throw new ArgumentException("Direction must be either up, down, left, or right.");
        }
    }

    // Return a reference to the GameObject of a door or wall
    public GameObject GetEdgeObstacle(GameObject space, String direction)
    {
        direction = direction.ToLowerInvariant();

        if (!space.tag.Equals(inFloorTag) && !space.tag.Equals(outFloorTag)) // Not a space game object
        {
            throw new ArgumentException("GameObject space must have tag " + inFloorTag + " or " + outFloorTag);
        }

        int[] c = FloorCoordinate(space);
        return GetEdgeObstacle(c[0], c[1], direction);
    }

    //Return a reference to all the firemen standing on the space
    public ArrayList GetFiremen(int x, int y)
    {
        return firemen[x, y];
    }
    public ArrayList GetFiremen(GameObject space)
    {
        int[] c = FloorCoordinate(space);
        return GetFiremen(c[0], c[1]);
    }

    //Return a reference to all space gameobjects adjacent to the space at coordinates [x,y]
    public List<GameObject> GetAdjacent(int x, int y)
    {
        List<GameObject> adj = new List<GameObject>();

        if (IsOnBoard(x + 1, y)) //Right
        {
            if (GetEdgeObstacle(x, y, "right") == null || GetEdgeObstacle(x, y, "right").GetComponent<EdgeObstacle>().IsPassable())
            {
                adj.Add(GetSpace(x + 1, y));
            }
        }
        if (IsOnBoard(x - 1, y)) //Left
        {
            if (GetEdgeObstacle(x, y, "left") == null || GetEdgeObstacle(x, y, "left").GetComponent<EdgeObstacle>().IsPassable())
            {
                adj.Add(GetSpace(x - 1, y));
            }
        }
        if (IsOnBoard(x, y - 1)) //Up
        {
            if (GetEdgeObstacle(x, y, "up") == null || GetEdgeObstacle(x, y, "up").GetComponent<EdgeObstacle>().IsPassable())
            {
                adj.Add(GetSpace(x, y - 1));
            }
        }
        if (IsOnBoard(x, y - 1)) //Down
        {
            if (GetEdgeObstacle(x, y, "down") == null || GetEdgeObstacle(x, y, "down").GetComponent<EdgeObstacle>().IsPassable())
            {
                adj.Add(GetSpace(x, y + 1));
            }
        }

        return adj;
    }

    //Return a reference to all Wall gameobjects adjacent to launch pad
    public List<GameObject> GetAdjacentWalls(GameObject space)
    {
        List<GameObject> walls = new List<GameObject>();
        String[] directions = { "up", "down", "left", "right" };
        int[] c = GetSpaceCoordinates(space);

        if (c == null)
        {
            throw new ArgumentException();
        }

        //Check if there is a wall in each direciton
        foreach (String direction in directions)
        {
            GameObject edge = GetEdgeObstacle(c[0], c[1], direction);
            if (edge != null && edge.tag == wallTag)
            {
                walls.Add(edge);
            }
        }

        return walls;
    }


    //-----------------------------+
    // END TURN LOGIC			   | : Check Win, Contact Server for Dice Roll, Advance Fire, Check Deaths/Knockouts, Check Loss, Extinguish Outside Fires, Contact Server for Dice Rolls Replenish POI
    //-----------------------------+

    // (1) - Check Win TODO: Exit current scene to winning scene. 
    public void CheckWin()
    {

        if (SavedPOI >= SavedWinThreshold)
        {
            //Load client into the win scene
            SceneManager.LoadScene("EndGameWin");
        }
    }

    // (2) - Execute fire advancement procedure based on a dice roll.
    public void AdvanceFire(int[] roll)
    {
        int x = roll[0];
        int y = roll[1];

        Debug.Log("Rolled " + x + " " + y);

        //Check for a fire on the rolled space 
        Space target = GetSpace(x, y).GetComponent<Space>();

        if (target.IncrementFire()) //Space returns true if an explosion should occur after incrementing
        {
            Explode(x, y);
        }

    }

    // Explosion logic
    private void Explode(int x, int y)
    {
        Debug.Log("Explosion at " + x + " " + y);
        int[] up = { x, y - 1 };
        int[] down = { x, y + 1 };
        int[] left = { x - 1, y };
        int[] right = { x + 1, y };
        int[][] coordinates = { up, down, left, right };
        String[] directions = { "up", "down", "left", "right" };

        // Explode in each direction
        for (int i = 0; i < directions.Length; i++)
        {
            //Check for wall first
            GameObject wall = GetEdgeObstacle(x, y, directions[i]);
            if (wall != null && !wall.GetComponent<EdgeObstacle>().IsPassable())
            {
                //Wall in the way, damage the wall instead
                wall.GetComponent<EdgeObstacle>().Damage();
                continue;
            }

            //No wall in the way, increment fire of the adjacent space in that direction
            Space adj = GetSpace(coordinates[i][0], coordinates[i][1]).GetComponent<Space>();
            if (adj.IncrementFire())
            {
                //The adjacent space already had a fire, send a shockwave in that direction.
                Shockwave(coordinates[i][0], coordinates[i][1], x, y);
            }


        }
    }

    private void Shockwave(int x, int y, int explodeX, int explodeY)
    {
        Debug.Log("Shockwave at " + x + " " + y);
        String direction;
        int xdiff = x - explodeX;
        int ydiff = y - explodeY;
        if (xdiff == -1)
        {
            direction = "left";
        }
        else if (xdiff == 1)
        {
            direction = "right";
        }
        else if (ydiff == 1)
        {
            direction = "down";
        }
        else
        {
            direction = "up";
        }
        //Continue in a straight line until reaching an non-fire space or wall 
        int i = 0;
        while (IsOnBoard(new int[] { (i * xdiff) + x, (i * ydiff) + y }))
        {

            //Check for wall in that direction first
            GameObject wall = GetEdgeObstacle(i * xdiff + x, i * ydiff + y, direction);
            if (wall != null)
            {
                if (!wall.GetComponent<EdgeObstacle>().IsPassable())
                {
                    wall.GetComponent<EdgeObstacle>().Damage();
                    break;
                }

            }

            //Move to next space over
            i++;
            Space space = GetSpace(i * xdiff + x, i * ydiff + y).GetComponent<Space>();
            if (!space.IncrementFire()) //Increment fire level. If it's already a fire continue
            {
                break;
            }
        }
    }

    // (2.1) Flashover - Increment Fire of Smoke Spaces adjacent to fire spaces
    public void Flashover()
    {
        //Iterate through every space
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //Flashovers originate from Fire, DFS the board for each space that is a fire
                FlashoverDFS(x, y);
            }
        }
    }

    // (2.2) DFS to increment smoke from an origin fire
    private void FlashoverDFS(int x, int y)
    {
        Space origin = floors[x, y].GetComponent<Space>();
        Stack<Space> dfsStack = new Stack<Space>();
        if (origin.status != SpaceStatus.Fire) return;
        Debug.Log("Flashover starting on " + x + " " + y);

        //Stack origin and loop until stack empties
        dfsStack.Push(origin);
        while (dfsStack.Count != 0)
        {
            Space curr = dfsStack.Peek(); //Always work with top of the stack
            int[] c = { curr.x, curr.y };
            Debug.Log("DFS Visiting: " + curr.x + " " + curr.y);

            //Check each adjacent space
            List<GameObject> adj = GetAdjacent(c[0], c[1]);
            bool end = true;
            foreach (GameObject s in adj)
            {
                Space adjSpace = s.GetComponent<Space>();
                Debug.Log("Checking " + adjSpace.x + " " + adjSpace.y + "Status: " + adjSpace.status);

                if (adjSpace.status == SpaceStatus.Smoke) //Fire spreads i.e. an edge exists
                {
                    adjSpace.IncrementFire(); //Ignite
                    dfsStack.Push(adjSpace); //Push and continue dfs on adjSpace
                    end = false;
                    break;
                }
            }
            //Reaching end of loop means there is no adjacent smoke, pop from stack, backtrack and continue dfs 
            if (end)
            {
                dfsStack.Pop();
            }
        }
    }

    // (3) - Check for Deaths and Knockouts TODO: Finish actual death and knockout movement sequence
    public void ResolveDeaths()
    {
        int currentDeaths = 0;
        //Iterate through all spaces and check if there are any firemen or POI's on unsafe spaces
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (floors[x, y].GetComponent<Space>().status != SpaceStatus.Fire) continue; //Ignore non-fire spaces

                //Get objects on x,y via BoardManager
                ArrayList localFiremen = firemen[x, y];
                List<GameObject> localPOI = POIManager.Instance.GetFromSpace(x, y);

                //Resolve Knockouts for Firemen
                if (localFiremen.Count > 0)
                {
                    //TODO: Fireman knockout logic using Fireman class
                }
                //Resolve Deaths for POI's
                if (localPOI.Count > 0)
                {
                    foreach (GameObject poi in localPOI)
                    {
                        currentDeaths++;
                        poi.GetComponent<POI>().Death();
                    }
                }

            }
        }
    }

    // (4) - Check for Loss TODO: Exit current scene to losing scene
    public void CheckLoss()
    {

        if (HouseHP == 0 || RemainingPOI == 0)
        {
            SceneManager.LoadScene("EndGameLoss");
        }
    }

    // (5) - Extinguish Outside Fires
    public void ExtinguishOutsideFires()
    {
        // Iterate through all spaces and for those that are outside, set to safe again.
        foreach (GameObject s in floors)
        {
            if (s.tag == outFloorTag) s.GetComponent<Space>().SetStatus(SpaceStatus.Safe);
        }
    }

    // (6) - Replenish POI a given amount onto the board. 
    public void ReplenishPOI(List<int[]> rolls)
    {
        foreach (int[] roll in rolls)
        {

        }
    }




    //-----------------------------+
    // LOCATORS - BASED ON VECTOR3 | : These methods translate Vector3 positions of game objects into coordinates
    //-----------------------------+

    // Use the corner of the board and the Vector3 of the floor GameObject to determine its 2D coordinate
    public int[] FloorCoordinate(GameObject floorObject)
    {
        int[] coordinates = new int[2];
        Vector3 position = floorObject.transform.position;
        //TODO: 4 and 1 are hardcoded for the family board
        if (floorObject.tag.Equals(inFloorTag))
        {
            coordinates[0] = (int)(Math.Abs((position.x - (houseCorner.x + 4)) / tileSize)) + 1; //x coordinate
            coordinates[1] = (int)(Math.Abs((position.z - (houseCorner.z - 4)) / tileSize)) + 1; //z coordinate
        }
        else
        {
            coordinates[0] = (int)(Math.Abs((position.x - (houseCorner.x)) / tileSize)); //x coordinate
            coordinates[1] = (int)(Math.Abs((position.z - (houseCorner.z)) / tileSize)); //z coordinate
        }

        return coordinates;
    }
    // Use the corner of the board and the Vector3 of the wall GameObject to determine its 2D coordinate
    public int[] EdgeCoordinate(GameObject edgeObject)
    {
        double wallOffset = 2;
        int[] coordinates = new int[2];
        Vector3 position = edgeObject.transform.position;
        bool vertical = (edgeObject.transform.rotation.y != 0); //Different calculation based on wall rotation.

        if (vertical) //Vertical Calculation
        {
            coordinates[0] = (int)Math.Abs((position.x + wallOffset - houseCorner.x) / tileSize);
            coordinates[1] = (int)Math.Abs((position.z - houseCorner.z) / tileSize);

        }
        else //Horizontal Calculation
        {
            coordinates[0] = (int)Math.Abs((position.x - houseCorner.x) / tileSize);
            coordinates[1] = (int)Math.Abs((position.z - wallOffset - houseCorner.z) / tileSize);
        }


        return coordinates;
    }
    // TODO: Doors were fucked with, update method of getting edge coordinate.

    public bool IsOutside(int[] c)
    {
        return (c[0] == 0 || c[0] == columns - 1 || c[1] == 0 || c[1] == rows - 1);
    }
    public bool IsOnBoard(int[] c)
    {
        return (c[0] > 0 || c[0] < columns - 1 || c[1] > 0 || c[1] < rows - 1);
    }
    public bool IsOutside(int x, int y)
    {
        return IsOutside(new int[] { x, y });
    }
    public bool IsOnBoard(int x, int y)
    {
        return IsOnBoard(new int[] { x, y });
    }


    //-----------------------------+
    // UNITY INIT				   | :
    //-----------------------------+

    // Generate fires on 6 random spaces
    void GenerateFiresRandom()
    {
        ArrayList rolls = new ArrayList();
        int index = 0;
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                rolls.Add(new int[2] { x, y });
            }
        }

        // Randomly pick 6 possible rolls and set their spaces to fire
        int possibleRolls = rolls.Count;
        int remainingRolls = 6;
        while (remainingRolls != 0)
        {
            // Get random coordinate
            Random r = new Random();
            int rInt = r.Next(0, possibleRolls);
            int[] roll = (int[])rolls[rInt];
            rolls.RemoveAt(rInt);
            // Set space at coordinate to fire 
            floors[roll[0], roll[1]].GetComponent<Space>().SetStatus(SpaceStatus.Fire);
            Debug.Log("Chosen for fire: " + floors[roll[0], roll[1]].name);

            possibleRolls--;
            remainingRolls--;
        }

    }

    // Generate fires on predetermined family mode spaces
    void GenerateFiresFamily()
    {
        GetSpace(2, 2).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(3, 2).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(2, 3).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(3, 3).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(4, 3).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(5, 3).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(4, 4).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(6, 5).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(7, 5).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
        GetSpace(6, 6).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
    }

    // Update board state based on the Vector3 positions of GameObjects already placed via Editor
    void LoadFromEnvironment()
    {

        //Setup all Inside Floors
        GameObject[] inFloorObj = GameObject.FindGameObjectsWithTag(inFloorTag);
        for (int i = 0; i < inFloorObj.Length; i++)  //Loop through inside floors and put them into the correct coordinate
        {
            int[] c = FloorCoordinate(inFloorObj[i]);

            if (IsOutside(c) || !IsOnBoard(c)) //Check if floorObj is at an invalid position
            {
                throw new InvalidPositionException();
            }

            floors[c[0], c[1]] = inFloorObj[i]; //Set the space at x,y to the floor object

        }


        //Setup all Outside Floors
        GameObject[] outFloorObj = GameObject.FindGameObjectsWithTag(outFloorTag);
        for (int i = 0; i < outFloorObj.Length; i++)
        {
            int[] c = FloorCoordinate(outFloorObj[i]);
            if (!IsOutside(c))
            {
                throw new InvalidPositionException();
            }
            floors[c[0], c[1]] = outFloorObj[i];
        }

        //Setup all Walls
        GameObject[] wallObj = GameObject.FindGameObjectsWithTag(wallTag);
        for (int i = 0; i < wallObj.Length; i++)
        {
            int[] c = EdgeCoordinate(wallObj[i]);
            if (!IsOnBoard(c))
            {
                throw new InvalidPositionException();
            }

            if (wallObj[i].transform.rotation.y != 0) // Left edge
            {
                leftEdge[c[0], c[1]] = wallObj[i];

            }
            else //Upper edge										
            {

                upperEdge[c[0], c[1]] = wallObj[i];
            }
        }

        //Setup all Doors TODO
        GameObject[] doorObj = GameObject.FindGameObjectsWithTag(inDoorTag);
        Debug.Log("Number of Doors: " + doorObj.Length);
        for (int i = 0; i < doorObj.Length; i++)
        {

            int[] c = EdgeCoordinate(doorObj[i]);
            if (!IsOnBoard(c))
            {
                throw new InvalidPositionException();
            }

            if (doorObj[i].transform.rotation.y != 0) // Left edge
            {
                leftEdge[c[0], c[1]] = doorObj[i];
            }
            else //Upper edge										
            {
                upperEdge[c[0], c[1]] = doorObj[i];
            }
        }
    }

    // Update board state based on GameObjects placed in the Scene
    void LoadFromScene()
    {
        //Spaces
        GameObject[] inSpaces = GameObject.FindGameObjectsWithTag(inFloorTag);
        for (int i = 0; i < inSpaces.Length; i++)
        {
            int x = inSpaces[i].GetComponent<Space>().x;
            int y = inSpaces[i].GetComponent<Space>().y;
            floors[x, y] = inSpaces[i];
        }
        GameObject[] outSpaces = GameObject.FindGameObjectsWithTag(outFloorTag);
        for (int i = 0; i < outSpaces.Length; i++)
        {
            int x = outSpaces[i].GetComponent<Space>().x;
            int y = outSpaces[i].GetComponent<Space>().y;
            floors[x, y] = outSpaces[i];
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag(wallTag);
        for (int i = 0; i < walls.Length; i++)
        {
            int x = walls[i].GetComponent<Wall>().x;
            int y = walls[i].GetComponent<Wall>().y;
            String direction = walls[i].GetComponent<Wall>().direction;
            if (direction.ToLowerInvariant().Equals("left")) leftEdge[x, y] = walls[i];
            else if (direction.ToLowerInvariant().Equals("up")) upperEdge[x, y] = walls[i];
            else throw new Exception(walls[i].name);

        }

        GameObject[] inDoors = GameObject.FindGameObjectsWithTag(inDoorTag);
        for (int i = 0; i < inDoors.Length; i++)
        {
            int x = inDoors[i].GetComponent<Door>().x;
            int y = inDoors[i].GetComponent<Door>().y;
            String direction = inDoors[i].GetComponent<Door>().direction;
            if (direction.ToLowerInvariant().Equals("left")) leftEdge[x, y] = inDoors[i];
            else if (direction.ToLowerInvariant().Equals("up")) upperEdge[x, y] = inDoors[i];
            else throw new InvalidPositionException();
        }

        GameObject[] outDoors = GameObject.FindGameObjectsWithTag(outDoorTag);
        for (int i = 0; i < outDoors.Length; i++)
        {
            int x = outDoors[i].GetComponent<Door>().x;
            int y = outDoors[i].GetComponent<Door>().y;
            String direction = outDoors[i].GetComponent<Door>().direction;
            if (direction.ToLowerInvariant().Equals("left")) leftEdge[x, y] = outDoors[i];
            else if (direction.ToLowerInvariant().Equals("up")) upperEdge[x, y] = outDoors[i];
            else throw new InvalidPositionException();
        }


    }

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        // Instantiate grids
        floors = new GameObject[columns, rows];
        leftEdge = new GameObject[columns, rows];
        upperEdge = new GameObject[columns, rows];
        ambulances = new GameObject[columns, rows];
        LoadFromScene();
        //GenerateFiresFamily();
        //AdvanceFire(new int[] { 1, 1 });
        //AdvanceFire(new int[] { 1, 1 });
        //AdvanceFire(new int[] { 1, 2 });
        GenerateFiresFamily();

        GameObject poi1 = POIManager.Instance.GeneratePOI(1, 4, true);
        poi1.GetComponent<POI>().Reveal();

    }
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

}

class InvalidPositionException : Exception
{
    public InvalidPositionException()
    {
    }
}

class SpaceNotOnBoardException : Exception
{
    public SpaceNotOnBoardException() { }
}

