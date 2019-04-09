using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, EdgeObstacle {
	public DoorState state;
	public int x;
	public int y;
	public string direction;

    public void SetState(DoorState s)
    {
        state = s;
        ToggleDoor();
    }

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
            this.GetComponent<DoorController>().InteractDoor();
        }
	}

	// Destroy the door
	public void Damage()
	{
		gameObject.GetComponent<DoorController>().damageDoor(); 
		if(state == DoorState.Destroyed)
		{
			throw new InvalidDoorOperationException();
		}
		else
		{
			state = DoorState.Destroyed;
		}
	}

	// Door passable if destroyed or open
	public bool IsPassable()
	{
		return (state == DoorState.Destroyed || state == DoorState.Open);
	}
	public bool IsDestroyed()
	{
		return state == DoorState.Destroyed;
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
