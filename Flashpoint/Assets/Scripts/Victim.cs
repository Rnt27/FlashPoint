using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Victim : POI
{
	// TODO: Need reference to the firefighter carrying me?


	// TODO: Handle animations and actual game object transformation for each method. (Potentially done in a controller)
	// Handle state change, animation and gameobject transformation for the POI 
	public void Move(string direction)
	{
		switch (direction)
		{
			case "up":
				if (BoardManager.Instance.IsOnBoard(x, y - 1)) y--;
				else throw new InvalidMoveException();
				break;
			case "down":
				if (BoardManager.Instance.IsOnBoard(x, y + 1)) y++;
				else throw new InvalidMoveException();
				break;
			case "right":
				if (BoardManager.Instance.IsOnBoard(x + 1, y)) x++;
				else throw new InvalidMoveException();
				break;
			case "left":
				if (BoardManager.Instance.IsOnBoard(x - 1, y)) y--;
				else throw new InvalidMoveException();
				break;
			default:
				break;
				throw new ArgumentException();
		}
	}

	public void InitPOI(int x, int y)
	{
		this.x = x;
		this.y = y;
		MoveTo(x, y);
		victim = true; 
	}

	//Disable inherited Reavel function
	new void Reveal()
	{
		return;
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
