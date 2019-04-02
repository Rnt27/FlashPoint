using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class POI : MonoBehaviour
{

	public int x { get; set; }
	public int y { get; set; }

	// TODO: Need reference to the firefighter carrying me?

	
	// TODO: Handle animations and actual game object transformation for each method. (Potentially done in a controller)
	// Handle state change, animation and gameobject transformation for the POI 
	public void Move(string direction)
	{
		switch (direction)
		{
			case "up":
				if(BoardManager.Instance.IsOnBoard(x, y-1)) y--;
				else throw new InvalidMoveException();
				break;
			case "down":
				if (BoardManager.Instance.IsOnBoard(x, y+1)) y++;
				else throw new InvalidMoveException();
				break;
			case "right":
				if (BoardManager.Instance.IsOnBoard(x+1, y)) x++;
				else throw new InvalidMoveException();
				break;
			case "left":
				if (BoardManager.Instance.IsOnBoard(x-1, y)) y--;
				else throw new InvalidMoveException();
				break;
			default:
				break;
				throw new ArgumentException();
		}
	}

	// POI is killed by fire. 
	public void Death()
	{
		//Any animations or transformations go here

		//Remove self from game and update state
		Destroy(gameObject);
		BoardManager.Instance.PoiDeath();
		BoardManager.Instance.CheckWin();
	}

	// Teleport POI to a certain space
	public void MoveTo(int x, int y)
	{
		if (BoardManager.Instance.IsOnBoard(x, y))
		{
			GameObject space = BoardManager.Instance.GetSpace(x, y);
			gameObject.transform.position = space.transform.position;
		}
		else
		{
			throw new InvalidPositionException();
		}
		
	}

	// Upon leaving the house, the POI is saved.
	private void OnTriggerEnter(Collider space)
	{
		if (space.gameObject.tag.Equals("OutsideTile")) //The POI collided with an outside tile
		{
			//Remove self from game
			Destroy(gameObject);

			//Update Game State and Check for a win
			BoardManager.Instance.PoiSaved();
			BoardManager.Instance.CheckWin();
		}
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

class InvalidMoveException : Exception
{
	public InvalidMoveException() { }
}