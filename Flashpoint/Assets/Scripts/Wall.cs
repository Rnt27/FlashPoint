using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, EdgeObstacle {
	public WallState state;
	public int x;
	public int y;
	public string direction;

	// Damage wall by incrementing state
	public void Damage()
	{
		
		switch (state)
		{
			case WallState.Intact: 
				SetState(state+1);
				break; 
			case WallState.Damaged: 
				SetState(state+1);
				break;
			case WallState.Destroyed: 
				//Nothing happens
				return;
			default:
				return;
		}
		//Alter appearance of the wall
		gameObject.GetComponent<WallController>().HitWall();
		Debug.Log(gameObject.GetComponent<WallController>().name);

		//Update HouseLife UI
		HouseLife.Instance.diminishHealth(); 

	}

	public WallState GetState()
	{
		return state;
	}
	public void SetState(WallState s)
	{
		state = s;
		// TODO: Adjust this.gameObject to take corresponding form 
		
	}

	// Wall is passable if it is destroyed
	public bool IsPassable()
	{
		return (state == WallState.Destroyed);
	}
	public bool IsDestroyed()
	{
		return state == WallState.Destroyed;
	}

	void Start()
	{
		//TODO
	}
	void Update()
	{
	}


	
}

public enum WallState
{
	Intact = 0,
	Damaged = 1,
	Destroyed = 2,
}
