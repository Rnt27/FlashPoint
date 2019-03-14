using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


 


    //-----------------------------+
    // PUBLIC API				   | : 
    //-----------------------------+

	//Return a reference to the Space script for coordinates x, y
    public Space GetSpace(int x, int y)
    {
	    int[] c = {x, y};
	    if (!IsOnBoard(c))
	    {
			throw new InvalidPositionException();
	    }

	    return (Space) floors[x, y].GetComponent( typeof(Space) );
    }

	//Return a reference to the EdgeObstacle script for coordinates x, y
	public EdgeObstacle GetEdgeObstacle(int x, int y, String direction)
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
			return (EdgeObstacle)upperEdge[x, y].GetComponent(typeof(EdgeObstacle));
		}
		else if (direction.Equals("left"))
		{
			return (EdgeObstacle)leftEdge[x, y].GetComponent(typeof(EdgeObstacle));
		}
		else if (direction.Equals("down"))
		{
			if (!IsOnBoard(new int[] {x, y + 1}))
			{
				return null;
			}
			return (EdgeObstacle) upperEdge[x, y + 1].GetComponent(typeof(EdgeObstacle));
;		}
		else if (direction.Equals("right"))
		{
			if(!IsOnBoard(new int[] {x+1, y}))
			{
				return null;
			}

			return (EdgeObstacle) leftEdge[x + 1, y].GetComponent(typeof(EdgeObstacle));
		}
		else
		{
			throw new ArgumentException("Direction must be either up, down, left, or right.");
		}
	}

	// Return a reference to the EdgeObstacle script for a given space GameObject
	public EdgeObstacle GetEdgeObstacle(GameObject space, String direction)
	{
		direction = direction.ToLowerInvariant();

		if (!space.tag.Equals(inFloorTag) && !space.tag.Equals(outFloorTag) ) // Not a space game object
		{
			throw new ArgumentException("GameObject space must have tag "+inFloorTag+" or "+outFloorTag);
		}

		int[] c = FloorCoordinate(space);
		return GetEdgeObstacle(c[0], c[1], direction);
	}

	// Returns a list of adjacent spaces

	//-----------------------------+
	// LOCATORS - BASED ON VECTOR3 | : These methods translate Vector3 positions of game objects into coordinates
	//-----------------------------+

	// Use the corner of the board and the Vector3 of the floor GameObject to determine its 2D coordinate
	public int[] FloorCoordinate(GameObject floorObject)
    {
        int[] coordinates = new int[2];
        Vector3 position = floorObject.transform.position;
        coordinates[0] = (int)((position.x - houseCorner.x) / tileSize); //x coordinate
        coordinates[1] = (int)((position.z - houseCorner.z) / tileSize); //y coordinate
        return coordinates;
    }
    // Use the corner of the board and the Vector3 of the wall GameObject to determine its 2D coordinate
    public int[] EdgeCoordinate(GameObject edgeObject)
    {
        double wallOffset = 2;
        int[] coordinates = new int[2];
        Vector3 position = edgeObject.transform.position;
        bool vertical = (edgeObject.transform.rotation.y == 90); //Different calculation based on wall rotation.

        if (vertical) //Vertical Calculation
        {
            coordinates[0] = (int)((position.x + wallOffset - houseCorner.x) / tileSize);
            coordinates[1] = (int)((position.y - houseCorner.y) / tileSize);

        }
        else //Horizontal Calculation
        {
            coordinates[0] = (int)((position.x - houseCorner.x) / tileSize);
            coordinates[1] = (int)((position.z - wallOffset - houseCorner.y) / tileSize);
        }

        return coordinates;
    }

    public bool IsOutside(int[] c)
    {
        return ((c[0] == 0 || c[0] == columns - 1) && (c[1] == 0 || c[1] == rows - 1));
    }
    public bool IsOnBoard(int[] c)
    {
        return (c[0] < 0 || c[0] > columns - 1 || c[1] < 0 || c[1] > rows - 1);
    }
    public bool IsOutside(int x, int y)
    {
	    return ((x == 0 || x == columns - 1) && (y == 0 || y == rows - 1));
	}
    public bool IsOnBoard(int x, int y)
    {
	    return (x < 0 || x > columns - 1 || y < 0 || y > rows - 1);
    }


	//-----------------------------+
	// UNITY INIT				   | :
	//-----------------------------+

	// Update board state based on the Vector3 positions of GameObjects already placed via Editor
	void LoadFromEnvironment()
	{

		//Setup all Inside Floors
		GameObject[] inFloorObj = GameObject.FindGameObjectsWithTag(inFloorTag);
		for (int i = 0; i < inFloorObj.Length; i++)  //Loop through inside floors and put them into the correct coordinate
		{
			int[] c = FloorCoordinate(inFloorObj[i]);

			if (IsOutside(c)) //Check if floorObj is at an invalid position
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
			if (IsOnBoard(c))
			{
				throw new InvalidPositionException();
			}

			if ((int)wallObj[i].transform.rotation.y == 90) // Left edge
			{
				leftEdge[c[0], c[1]] = wallObj[i];
			}
			else //Upper edge										
			{
				upperEdge[c[0], c[1]] = wallObj[i];
			}
		}

		//Setup all Doors
		GameObject[] doorObj = GameObject.FindGameObjectsWithTag(doorTag);
		for (int i = 0; i < doorObj.Length; i++)
		{
			int[] c = FloorCoordinate(doorObj[i]);
			if (IsOnBoard(c))
			{
				throw new InvalidPositionException();
			}

			if ((int)doorObj[i].transform.rotation.y == 90) // Left edge
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
}
    // Use this for initialization
    void Start()
    {
	    LoadFromEnvironment();
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


