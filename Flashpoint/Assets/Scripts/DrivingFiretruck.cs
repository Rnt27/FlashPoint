using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingFiretruck : MonoBehaviour
{
    Transform startwhere;

    

    bool right = true;

    public bool south = true;
    public bool west = false;
    public bool north = false;
    public bool east = false;
        
    public Transform firetruckSouth;
    public Transform firetruckWest;
    public Transform firetruckNorth;
    public Transform firetruckEast;

    // Start is called before the first frame update
    void Start()
    {

        firetruckSouth.gameObject.SetActive(true);
        firetruckWest.gameObject.SetActive(false);
        firetruckNorth.gameObject.SetActive(false);
        firetruckEast.gameObject.SetActive(false);

}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            moveLeft();
        }

        if (Input.GetKeyDown("k"))
        {
            moveRight();
        }
    }

    public void moveLeft()
    {
                        
        if (south)
        {

            firetruckSouth.gameObject.SetActive(false);
            firetruckWest.gameObject.SetActive(true);

            south = false;
            west = true;
                        
        }
        else if (west)
        {
            firetruckWest.gameObject.SetActive(false);
            firetruckNorth.gameObject.SetActive(true);
        
            west = false;
            north = true;
        }
        else if (north)
        {
            firetruckNorth.gameObject.SetActive(false);
            firetruckEast.gameObject.SetActive(true);

            north = false;
            east = true;

        }
        else if (east)
        {
            firetruckSouth.gameObject.SetActive(true);
            firetruckEast.gameObject.SetActive(false);

            east = false;
            south = true;
        }

    }

    public void moveRight()
    {

        if (south)
        {

            firetruckSouth.gameObject.SetActive(false);
            firetruckEast.gameObject.SetActive(true);

            south = false;
            east = true;

        }
        else if (east)
        {
            firetruckEast.gameObject.SetActive(false);
            firetruckNorth.gameObject.SetActive(true);

            east = false;
            north = true;
        }
        else if (north)
        {
            firetruckNorth.gameObject.SetActive(false);
            firetruckWest.gameObject.SetActive(true);

            north = false;
            west = true;

        }
        else if (west)
        {
            firetruckSouth.gameObject.SetActive(true);
            firetruckWest.gameObject.SetActive(false);

            west = false;
            south = true;
        }

    }
}
