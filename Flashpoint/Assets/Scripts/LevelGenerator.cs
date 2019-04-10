using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{
	//RNG for the level to be seeded in Init()
	Random rand; 

	public static LevelGenerator Instance = null;
	//Standard Flashpoint Board Size
	public const int columns = 10;
	public const int rows = 8;
	public const float gridSize = 4;

	//Hazard Counts based on difficulty
	int numExplosions;
	int numHazmats;
	int numHotspots;

	//Instantiate Board GameObjects using these prefabs
	public GameObject floorPrefab;
	public GameObject doorPrefab;
	public GameObject wallPrefab;
	public GameObject[] fire;
	public GameObject blankPrefab;


	//Keep track of # of objects for naming purposes
	int numDoors = 0; 

	//The parent gameobject of all the ones to be created

	//Managers

	//Scene to load into once the board is made
	Scene template = SceneManager.GetSceneByName("RandomTemplate");
	
	//Initialize board details based on difficulty and seed. 
	public void Init(Difficulty d, int seed)
	{
		switch (d)
		{
			case Difficulty.Family: 
				numExplosions = 3;
				numHotspots = 0;
				numHazmats = 0;
				break;
			case Difficulty.Recruit:
				numExplosions = 3;
				numHotspots = 3;
				numHazmats = 3;
				break;
			case Difficulty.Veteran:
				numExplosions = 3;
				numHotspots = 3;
				numHazmats = 4;
				break;
			case Difficulty.Heroic:
				numExplosions = 4;
				numHotspots = 4;
				numHazmats = 5; 
				break;
		}
		SceneManager.LoadScene("RandomTemplate"); //Switch Scenes to the template

		rand = new Random(seed); //Initialize the RNG.
	}

	//Add a random door on each side of the board
	public void GenerateEntrances()
	{
		/**
		 * Up: y = 1 up 
		 * Down: y = 7 up
		 * Left: x = 1 left
		 * Right: x = 9 left 
		 **/

		string[] directions = { "up", "up", "left", "left" };
		int[] up = new int[] { rand.Next(1, 9) ,1};
		int[] down = new int[] { rand.Next(1, 9), 7 };
		int[] left = new int[] { 1, rand.Next(1, 6) };
		int[] right = new int[] { 9, rand.Next(1, 6) };
		int[][] rolls = new int[][] { up, down, left, right };
		
		//Add a door to each side of the house
		for(int i = 0; i < rolls.Length; i++)
		{
			int x = rolls[i][0];
			int y = rolls[i][1];
			NewDoor(x, y, directions[i], "DoorOutside");
		}
		
	}

	//Delete the wall at x, y and replace it with a door
	public GameObject NewDoor(int x, int y, string direction, string tag)
	{
		//Work with a set of edges dependent on inputted direction
		GameObject[,] edges;
		if (direction.Equals("left")) edges = BoardManager.Instance.leftEdge;
		else if (direction.Equals("up")) edges = BoardManager.Instance.upperEdge;
		else throw new ArgumentException(); 

		//Create the new door and position it 
		GameObject newDoor = Instantiate(doorPrefab);
		newDoor.transform.position = edges[x, y].transform.position;

		//Add tag and name to GameObject
		newDoor.tag = tag;
		newDoor.name = tag + " (" + numDoors + ")";
		
		//Add Door, DoorController , BoxCollider scripts to the door
		Door dScript = newDoor.AddComponent<Door>();
		dScript.x = x;
		dScript.y = y;
		dScript.ToggleDoor();

		DoorController dcScript = newDoor.AddComponent<DoorController>();
		dcScript.myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

		BoxCollider bc = newDoor.AddComponent<BoxCollider>();

		//Overwrite the old wall with the new door
		Destroy(edges[x, y]);
		edges[x, y] = newDoor; 

		return newDoor;
	}

	


	//Place a floor at coordinate x, y 
	GameObject NewFloor(int x, int y, int id)
	{
		string tag;
		if(BoardManager.Instance.IsOutside(x, y))
		{
			tag = "InsideTile";
		}
		else
		{
			tag = "OutsideTile";
		}
		GameObject newFloor = Instantiate(floorPrefab);
		newFloor.name = tag + " (" + id + ")";
		newFloor.tag = tag;
		newFloor.transform.position = FindPositionFloor(x, y);

		newFloor.AddComponent<BoxCollider>();
		newFloor.AddComponent<Space>();


		return newFloor;	
	}

	//Return the Vector3 of where a floor at x, y should go.
	Vector3 FindPositionFloor(int x, int y)
	{
		Vector3 position = new Vector3();
		position.x = x * gridSize;
		position.y = 0 * gridSize;
		position.z = y * gridSize;
		return position; 
	}


	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this; 
		}

		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {	

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
