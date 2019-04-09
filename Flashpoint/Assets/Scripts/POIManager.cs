using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class POIManager : MonoBehaviour
{
	public int numVictims = 12;
	public int numFalseAlarms = 6;
	public int maxOnBoard = 3; 
	public static POIManager Instance = null;
	public GameObject[] victimPrefabs;
	public GameObject poiPrefab; 

	List<GameObject> pois;

	public void AddPOI(GameObject poi)
	{
		pois.Add(poi);
	}
	public bool RemovePOI(GameObject poi)
	{
		return pois.Remove(poi);
	}

	//Get the number of missing POI's
	public int NumMissing()
	{
		return 3 - pois.Count;
	}

	//Roll based on the pieces left in the bag if the next piece will be a victim or false alarm
	public bool RollVictim()
	{
		Random r = new Random();
		int roll = r.Next(1, numVictims + numFalseAlarms);
		if (roll >= 1 && roll <= numVictims) return true;
		else return false;
	}

	// Create a new POI and place it on the board on space x,y
	public GameObject GeneratePOI(int x, int y, bool victimRoll)
	{
		if (numVictims + numFalseAlarms == 0) //No more POI's left in the bag 
		{
			return null; 
		}
		//If it's a victim, decrease the total amount of victims left "in the bag"
		if (victimRoll) numVictims--;
		else numFalseAlarms--;

		GameObject newPOI = Instantiate(poiPrefab);

		//Randomly choose if the POI is a victim or not. (Without replacement)
		

		//Add POI script to the gameObject and move it to the desired location.
		newPOI.AddComponent<POI>();
		newPOI.GetComponent<POI>().InitPOI(x, y, victimRoll);
		//Add the poi to our list
		pois.Add(newPOI);

		return newPOI;
	}

	public GameObject PlaceVictim(int x, int y)
	{
		//Choose a random prefab for the new victim
		System.Random r = new System.Random();
		GameObject newVictim = Instantiate(victimPrefabs[r.Next(0, victimPrefabs.Length)]);

		//Add Victim script to the gameObject and move it to the desired location.
		newVictim.AddComponent<Victim>();
		newVictim.GetComponent<Victim>().InitPOI(x, y);
		//Add victim to our list
		pois.Add(newVictim);

		return newVictim;
	}

	// Return a list of POI's that are standing on a certain space. 
	public List<GameObject> GetFromSpace(int x, int y)
	{
		List<GameObject> onSpace = new List<GameObject>(); 
		foreach(GameObject p in pois)
		{
			if(p.GetComponent<POI>().x == x && p.GetComponent<POI>().y == y)
			{
				onSpace.Add(p);
			}
		}

		return onSpace;
	}
	public List<GameObject> GetFromSpace(GameObject space)
	{
		int[] c= BoardManager.Instance.GetSpaceCoordinates(space);
		return GetFromSpace(c[0], c[1]);
	}

	private void Awake()
	{
		//Singleton
		if(Instance == null)
		{
			Instance = this;
		}
		pois = new List<GameObject>();
		
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
