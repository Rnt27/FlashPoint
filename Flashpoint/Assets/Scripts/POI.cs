using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class POI : MonoBehaviour
{

	public int x;
	public int y;
	public bool victim;


	// POI is killed by fire. 
	public void Death()
	{
		//Any animations or transformations go here


		//TODO: Inidicate to players whether or not the POI was a victim.
		//Remove self from game and update state

		if (victim)
		{
			POIUI.Instance.increaseDeath(); 
		}
		//Remove self from POIManager
		POIManager.Instance.RemovePOI(gameObject);
		Destroy(gameObject);
	}

	// Teleport POI to a certain space
	public void MoveTo(int x, int y)
	{
		if (BoardManager.Instance.IsOnBoard(x, y))
		{
			GameObject space = BoardManager.Instance.GetSpace(x, y);
			gameObject.transform.position = space.transform.position;
			gameObject.transform.position += new Vector3(0, 1, 0);
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
			//Update Game State and Check for a win
			POIUI.Instance.increaseVictory(); 

			//Remove self from game and POIManager
			POIManager.Instance.RemovePOI(gameObject);
			Destroy(gameObject);
		}
		
	}

	// TODO: Reveal the identity of the POI to the players
	public GameObject Reveal()
	{
		//Remove self from the POIManager
		POIManager.Instance.RemovePOI(gameObject);

		if (!victim)
		{
			//TODO: Animation/Message to indicate that the POI is a false alarm 
			POIManager.Instance.RemovePOI(gameObject);
			POIManager.Instance.numFalseAlarms--;
			Destroy(gameObject);
			return null;
		}
		else
		{
			GameObject newVictim = POIManager.Instance.PlaceVictim(x, y);
			POIManager.Instance.RemovePOI(gameObject);
			Destroy(gameObject);
			return newVictim;
		}
	}

	public void InitPOI(int x, int y, bool victim)
	{
		this.x = x;
		this.y = y;
		this.victim = victim;
		MoveTo(x,y);
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