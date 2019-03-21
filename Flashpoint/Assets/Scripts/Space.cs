using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour {
	public SpaceStatus status;

	public GameObject[] fireObject = new GameObject[3];
	

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
	
	// Increases the fire level (SpaceStatus) of the space.
	// Returns true if explosion should occur. 
	public bool IncrementFire()
	{ 
		bool explosion = false; 

		switch (status){
			case SpaceStatus.Safe:
				SetStatus(status++);
				break;
			case SpaceStatus.Smoke:
				SetStatus(status++);
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
				SetStatus(status--);
				break;
			case SpaceStatus.Fire:
				SetStatus(status--);
				break;
			default:
				break;
		}

		return success;
	}

	// Set the status of the space to s
	public void SetStatus(SpaceStatus s)
	{	
		Debug.Log(gameObject.name + "set as "+ s.ToString());
		Debug.Log(gameObject.transform.position);
		this.status = s;
		Instantiate(fireObject[(int)s]).SetActive(true);
		fireObject[(int) s].transform.position = gameObject.transform.position;
	
		// Disable irrelevant fire objects
		for(int i = (int) SpaceStatus.Safe; i < (int) SpaceStatus.Fire; i++)
		{
			if(i != (int) s)
			{
				fireObject[i].SetActive(false);
			}
		}

	}


	void Awake()
	{

		for (int i = 0; i < fireObject.Length; i++) // Places all fireobjects appropriately and disable. 
		{
			SetStatus(SpaceStatus.Safe); // Default initialization of space is Safe
		}

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
