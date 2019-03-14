using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, EdgeObstacle {
	DoorState state;


	// Open/Close the door 
	public void ToggleDoor()
	{
		if (state == DoorState.Destroyed)
		{
			//TODO: Cannot toggle destroyed door
			throw new InvalidDoorOperationException();
		}
		else
		{
			state = 1 - state;
		}
	}

	// Destroy the door
	public void Damage()
	{
		if(state == DoorState.Destroyed)
		{
			throw new InvalidDoorOperationException();
		}
		else
		{
			state = DoorState.Destroyed;
		}
	}

	// Wall is passable if it is destroyed
	public bool IsPassable()
	{
		return (state == DoorState.Destroyed || state == DoorState.Open);
	}

	void Start()
	{
		state = DoorState.Closed;
	}

	void Update()
	{

	}

	
}

public enum DoorState
{
	Closed = 0,
	Open = 1,
	Destroyed = 2,
}

public class InvalidDoorOperationException : System.Exception
{
	public InvalidDoorOperationException()
	{

	}
}
