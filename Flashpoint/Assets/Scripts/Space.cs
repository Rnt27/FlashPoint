using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour {
	public SpaceStatus status;

	public GameObject[] fireObject = new GameObject[3];
	public GameObject[] fireInstances;

	public int x;
	public int y;


	public Space(SpaceStatus status)
	{
		switch (status)
		{
			case SpaceStatus.Safe:
				this.status = status;
				// Set the fire 
				break;
			case SpaceStatus.Smoke:
				this.status = status;
				break;
			case SpaceStatus.Fire:
				this.status = status;
				break;
			default: // Fire
				this.status = SpaceStatus.Safe;
				break;
		}
	}

	public Space(SpaceStatus status, int x, int y)
	{
		switch (status)
		{
			case SpaceStatus.Safe:
				this.status = status;
				// Set the fire 
				break;
			case SpaceStatus.Smoke:
				this.status = status;
				break;
			case SpaceStatus.Fire:
				this.status = status;
				break;
			default: // Fire
				this.status = SpaceStatus.Safe;
				break;
		}

		this.x = x;
		this.y = y;
	}

    public Space()
    {
    }

    // Increases the fire level (SpaceStatus) of the space.
    // Returns true if explosion should occur. 
    public bool IncrementFire()
	{ 
		bool explosion = false; 

		switch (status){
			case SpaceStatus.Safe:
				BoardManager.Instance.Highlight(gameObject);
				SetStatus(status+1);
				break;
			case SpaceStatus.Smoke:
				BoardManager.Instance.Highlight(gameObject);
				SetStatus(status+1);
				break;
			case SpaceStatus.Fire:
				// Explosion should occur
				explosion = true;
				break;
			default:
				break;
		}

		return explosion;
	}

	// Decrease SpaceStatus and update the fire GameObject overlay
	// Returns false if nothing happens.
	public bool DecrementFire()
	{
		bool success = true;
		switch (status)
		{
			case SpaceStatus.Safe:
				// Nothing happens
				success = false;
				break;
			case SpaceStatus.Smoke:
				SetStatus(status-1);
				break;
			case SpaceStatus.Fire:
				SetStatus(status-1);
				break;
			default:
				break;
		}

		return success;
	}

	// Set the status of the space to s
	public void SetStatus(SpaceStatus s)
	{	
		this.status = s;
		fireInstances[(int) s] = Instantiate(fireObject[(int) s]);
		fireInstances[(int)s].SetActive(true);
		fireInstances[(int) s].transform.position = gameObject.transform.position;
	
		// Disable irrelevant fire objects
		for(int i = (int) SpaceStatus.Safe; i <= (int) SpaceStatus.Fire; i++)
		{
			if(i != (int) s)
			{
				Destroy(fireInstances[i]);
				
			}
		}

	}

	//Check if another Space or EdgeObstacle is adjacent
	public bool IsAdjacent(GameObject s)
	{
		//Check if s exists in either of the adjacency lists returned by the BoardManager.
		List<GameObject> adjSpaces = BoardManager.Instance.GetAdjacent(this.x, this.y);
		List<GameObject> adjWalls = BoardManager.Instance.GetAdjacentWalls(gameObject);

		foreach(GameObject space in adjSpaces)
		{
			Debug.Log("Checking if space " + space.name + " is adjacent to input " + s.name);
			if(s == space && space != null)
			{
				Debug.Log("Space true.");
				return true;
			}
		}

		foreach(GameObject wall in adjWalls)
		{
			Debug.Log("Checking if wall "+wall.name+ "is adjacent to input "+s.name);
			if(s == wall && wall != null)
			{
				Debug.Log("Wall true.");
				return true; 
			}
		}

		//s was not found in any adjacent list, not adjacent.
		Debug.Log("Returning false.");
		return false;
	}

	

	void Awake()
	{
		fireInstances = new GameObject[3];
		//SetStatus(SpaceStatus.Safe);
	}
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		
	}
}

public enum SpaceStatus
{
	Safe = 0,
	Smoke = 1,
	Fire = 2,
}
