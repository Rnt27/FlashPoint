using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/**
 * Developer: Alex
 * Purpose: Manager for the board and all its spaces/pieces and work with GameManager
 **/
public class BoardManager : MonoBehaviour
{

	static String inFloorTag = "InsideTile";
	static String outFloorTag = "OutsideTile";
	static String wallTag = "Wall";
	static String doorTag = "Door";

	public static BoardManager Instance = null;
    public int columns = 10;
    public int rows = 8;

    public double tileSize = 4; // 4x4 size of every grid

    public Vector3 houseCorner = new Vector3(-16f, 0f, 16f);

    GameObject[,] floors;
    GameObject[,] leftEdge;
    GameObject[,] upperEdge;
    GameObject[,] vehicles;
	ArrayList[,] firemen;
	ArrayList[,] pois;

 


    //-----------------------------+
    // PUBLIC API				   | : 
    //-----------------------------+

	//Return a reference to the GameObject of a space (launch pad) for coordinates x, y
    public GameObject GetSpace(int x, int y)
    {
	    int[] c = {x, y};
	    if (!IsOnBoard(c))
	    {
			throw new InvalidPositionException();
	    }

	    return floors[x, y];
    }

	//Return a reference to the GameObject of a door/wall for coordinates x, y
	public GameObject GetEdgeObstacle(int x, int y, String direction)
	{
		direction = direction.ToLowerInvariant();

		int[] c = {x, y};
		if (!IsOnBoard(c))
		{
			throw new InvalidPositionException();
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
			if (!IsOnBoard(new int[] {x, y + 1}))
			{
				return null;
			}
			return upperEdge[x, y + 1];
;		}
		else if (direction.Equals("right"))
		{
			if(!IsOnBoard(new int[] {x+1, y}))
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

		if (!space.tag.Equals(inFloorTag) && !space.tag.Equals(outFloorTag) ) // Not a space game object
		{
			throw new ArgumentException("GameObject space must have tag "+inFloorTag+" or "+outFloorTag);
		}

		int[] c = FloorCoordinate(space);
		return GetEdgeObstacle(c[0], c[1], direction);
	}

	//Return a reference to all the POI's standing on the space
	public ArrayList GetPOI(int x, int y)
	{
		return pois[x, y];
	}
	public ArrayList GetPOI(GameObject space)
	{
		int[] c = FloorCoordinate(space);
		return GetPOI(c[0], c[1]);
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


	//-----------------------------+
	// FIRE ADVANCEMENT			   | : 
	//-----------------------------+

	// Execute fire advancement procedure based on a dice roll.
	public void AdvanceFire(int[] roll)
	{
		int x = roll[0];
		int y = roll[1];

		//Check for a fire on the rolled space 
		Space target = GetSpace(x, y).GetComponent<Space>();

		if (target.IncrementFire()) //Space returns true if an explosion should occur after incrementing
		{
			//Explosion logic
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
			Debug.Log("Vert");	
            coordinates[0] = (int)Math.Abs((position.x + wallOffset - houseCorner.x) / tileSize);
            coordinates[1] = (int)Math.Abs((position.z - houseCorner.z) / tileSize);

        }
        else //Horizontal Calculation
        {
			Debug.Log("Horz");
            coordinates[0] = (int)Math.Abs((position.x - houseCorner.x) / tileSize);
            coordinates[1] = (int)Math.Abs((position.z - wallOffset - houseCorner.z) / tileSize);
        }

        return coordinates;
    }

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
	    return IsOutside(new int[] {x, y});
    }
    public bool IsOnBoard(int x, int y)
    {
	    return IsOnBoard(new int[] {x, y});
    }


	//-----------------------------+
	// UNITY INIT				   | :
	//-----------------------------+

	// Generate fires on 6 random spaces
	void GenerateFiresRandom()
	{
		ArrayList rolls = new ArrayList();
		int index = 0;
		for (int x = 1; x < columns-1; x++)
		{
			for (int y = 1; y < rows-1; y++)
			{
				rolls.Add(new int[2]{x,y});
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
			int[] roll = (int[]) rolls[rInt];
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
		GetSpace(2,2).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
		GetSpace(3,2).GetComponent<Space>().SetStatus(SpaceStatus.Fire);
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

			Debug.Log(inFloorObj[i].name+": "+ inFloorObj[i].transform.position.x + " " + inFloorObj[i].transform.position.z);
			Debug.Log(c[0] + " " + c[1]);
			if (IsOutside(c) || !IsOnBoard(c)) //Check if floorObj is at an invalid position
			{
				Debug.Log(IsOnBoard(c));
				throw new InvalidPositionException();
			}

			floors[c[0], c[1]] = inFloorObj[i]; //Set the space at x,y to the floor object
	
		}

		Debug.Log("Done Inside.");

		//Setup all Outside Floors
		GameObject[] outFloorObj = GameObject.FindGameObjectsWithTag(outFloorTag);
		for (int i = 0; i < outFloorObj.Length; i++)
		{
			int[] c = FloorCoordinate(outFloorObj[i]);
			Debug.Log(outFloorObj[i].name + ": " + outFloorObj[i].transform.position.x + " " + outFloorObj[i].transform.position.z);
			Debug.Log(c[0] + " " + c[1]);
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
			Debug.Log(wallObj[i].name + ": " + wallObj[i].transform.position.x + " " + wallObj[i].transform.position.z);
			Debug.Log(c[0] + " " + c[1]);
			if (!IsOnBoard(c))
			{
				throw new InvalidPositionException();
			}

			if (wallObj[i].transform.rotation.y != 0) // Left edge
			{
				leftEdge[c[0], c[1]] = wallObj[i];
				Debug.Log(wallObj[i].name + " VERT!!!1");

			}
			else //Upper edge										
			{
				Debug.Log("HORIZONTAL");

				upperEdge[c[0], c[1]] = wallObj[i];
			}
		}

		Debug.Log("Wall finished.");

		//Setup all Doors
		GameObject[] doorObj = GameObject.FindGameObjectsWithTag(doorTag);
		for (int i = 0; i < doorObj.Length; i++)
		{
		
			int[] c = EdgeCoordinate(doorObj[i]);
			Debug.Log(doorObj[i].name + ": " + doorObj[i].transform.position.x + " " + doorObj[i].transform.position.z);
			Debug.Log(c[0] + " " + c[1]);
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
	    vehicles = new GameObject[columns, rows];
	    Debug.Log("Hello");
		LoadFromEnvironment();
	    GenerateFiresFamily();
	}
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update() {

    }

}

class InvalidPositionException : Exception
{
    public InvalidPositionException()
    {
    }
}


