using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFirefighter : MonoBehaviour{

    private FirefighterController firefighter;
    public bool placeFirefighterPanel;
    public GameObject mainGamePanel;
    private GameObject tileHighlight;
    private Selectable[] tiles;


    private void Start()
    {
        
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        firefighter = FindObjectOfType<FirefighterController>();
        
        //currently spawning firefighters
        placeFirefighterPanel = true;
    }


    void Update()
    {
        MouseOverLocation();

        //Place firefighter only if he has not yet done so
        if (Input.GetMouseButtonDown(0) && firefighter.gameObject.activeSelf && placeFirefighterPanel)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //If we click on outside Tiles
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "OutsideTile")
            {

                //Spawn firefighter on that tile
                firefighter.Spawn(hit.transform.gameObject.transform.position);

                firefighter.setTile(hit.transform.gameObject.GetComponent<Selectable>());

                placeFirefighterPanel = false;

            }

        }

        //Click to move only to adjacent tile
        else if (Input.GetMouseButtonDown(0) && !placeFirefighterPanel)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //If we click on outside Tiles
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "OutsideTile" && firefighter.getTile().isAdjacent(hit.transform.gameObject.GetComponent<Selectable>()))
            {

                //Not able to move through walls, but yes through doors
                if (!firefighter.getTile().compareWalls(hit.transform.gameObject.GetComponent<Selectable>()))
                {
                    //no diagonal
                    if (!firefighter.isDiagonal(hit.transform.gameObject.GetComponent<Selectable>()))
                    {
                        //Move firefighter one tile
                        firefighter.setTile(hit.transform.gameObject.GetComponent<Selectable>());
                        firefighter.moving = true;

                    }
                }
            }
            else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "InsideTile" && firefighter.getTile().isAdjacent(hit.transform.gameObject.GetComponent<Selectable>()))
            {
                //Not able to move through walls, but yes through doors
                if (!firefighter.getTile().compareWalls(hit.transform.gameObject.GetComponent<Selectable>()))
                {
                    //no diagonal
                    if (!firefighter.isDiagonal(hit.transform.gameObject.GetComponent<Selectable>()))
                    {
                        //Move firefighter one tile
                        firefighter.setTile(hit.transform.gameObject.GetComponent<Selectable>());
                        firefighter.moving = true;

                    }
                }
            }

            //hitting a wall
            else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Wall" && hit.transform.gameObject.GetComponent<WallController>().containTile(firefighter.getTile()))
            {

                //Move firefighter one tile
                hit.transform.gameObject.GetComponent<WallController>().hitWall();

            }
        }
    }
       
    private void MouseOverLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);
            //Debug.Log(gridPoint);
        }
    }

    public static bool Between( int num, int lower, int upper, bool inclusive = false)
    {
        return inclusive
            ? lower <= num && num <= upper
            : lower < num && num < upper;
    }

}



