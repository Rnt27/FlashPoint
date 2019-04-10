using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingAmbulance : MonoBehaviour
{
    
    bool south = true;
    bool west = false;
    bool north = false;
    bool east = false;

    public Transform firetruckSouth;
    public Transform firetruckWest;
    public Transform firetruckNorth;
    public Transform firetruckEast;

    public Transform tileSouthA;
    public Transform tileSouthB;

    public Transform tileWestA;
    public Transform tileWestB;

    public Transform tileNorthA;
    public Transform tileNorthB;

    public Transform tileEastA;
    public Transform tileEastB;

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
        //to test movement
        /*if (Input.GetKeyDown("j"))
        {
            moveLeft();
        }

        if (Input.GetKeyDown("k"))
        {
            moveRight();
        }*/
    }

    //moves vehicle left
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

    //moves vehicle right
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

    //checks if tile is good to access the vehicle
    public bool onTile(Space tile)
    {

        if (south)
        {

            if (tileSouthA == tile || tileSouthB == tile) return true;
            else
            {
                return false;
            }

        }
        else if (east)
        {
            if (tileEastA == tile || tileEastB == tile) return true;
            else
            {
                return false;
            }
        }
        else if (north)
        {
            if (tileNorthA == tile || tileNorthB == tile) return true;
            else
            {
                return false;
            }

        }
        else if (west)
        {
            if (tileWestA == tile || tileWestB == tile) return true;
            else
            {
                return false;
            }
        }

        return false;

    }
}
