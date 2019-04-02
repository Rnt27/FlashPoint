using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIManager : MonoBehaviour
{

	public static POIManager Instance = null;

	public GameObject[] poiPrefabs;

	List<GameObject> pois;

	public void AddPOI(GameObject poi)
	{
		pois.Add(poi);
	}
	public bool RemovePOI(GameObject poi)
	{
		return pois.Remove(poi);
	}

	// Create a new POI and place it on the board on space x,y
	public GameObject GeneratePOI(int x, int y)
	{
		//Choose a random prefab for the POI
		System.Random r = new System.Random();
		GameObject newPOI = Instantiate(poiPrefabs[r.Next(0, poiPrefabs.Length)]);
		
		//Add POI script to the gameObject and move it to the desired location.
		newPOI.AddComponent<POI>();
		newPOI.GetComponent<POI>().MoveTo(x, y);

		//Add the poi to our list
		pois.Add(newPOI);

		return newPOI;
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
