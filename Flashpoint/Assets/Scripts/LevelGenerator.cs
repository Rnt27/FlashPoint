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
	public void GenerateDoors()
	{
	
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
