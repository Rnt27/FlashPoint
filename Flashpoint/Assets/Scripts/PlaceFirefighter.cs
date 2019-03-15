using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFirefighter : MonoBehaviour{

    public FirefighterController firefighter;
    public bool placeFirefighterPanel;
    public GameObject mainGamePanel;
    private GameObject tileHighlight;
    private Selectable[] tiles;

    public float keyDelay = 3f;  // 1 second
    private float timePass = 0f;

    public FirefighterController[] firefighters;

    private void Start()
    {
                
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);

        firefighters = FindObjectsOfType<FirefighterController>();

        firefighters[0].myTurn = true;

        firefighter = firefighters[0];
        
        //currently spawning firefighters
        placeFirefighterPanel = true;

    }


    void Update()
    {
        //TurnFirefighter();

//        if (firefighter.myTurn) { 

        MouseOverLocation();

        //changing turns
        if (Input.GetKey(KeyCode.UpArrow) && Cursor.visible)
        {
            ChangeTurn();
        }

        //Place firefighter only if he has not yet done so
        if (Input.GetMouseButtonDown(0) && firefighter.gameObject.activeSelf && !firefighter.spawned)
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
            else if (Input.GetMouseButtonDown(0) && firefighter.spawned)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //If we click on outside Tiles
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "OutsideTile" && firefighter.getTile().isAdjacent(hit.transform.gameObject.GetComponent<Selectable>()))
                {

                    //Not able to move through walls, but yes through doors
                    if (!firefighter.getTile().compareWalls(hit.transform.gameObject.GetComponent<Selectable>()) && !firefighter.getTile().compareClose(hit.transform.gameObject.GetComponent<Selectable>()))
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

                    firefighter.targetWall = hit.transform.gameObject.GetComponent<WallController>();

                    firefighter.punch = true;
                    /*
                    //Damage wall
                    hit.transform.gameObject.GetComponent<WallController>().hitWall();
                    */
                }

                else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "DoorInside" && hit.transform.gameObject.GetComponent<DoorController>().containTile(firefighter.getTile()))
                {
                    firefighter.targetDoor = hit.transform.gameObject.GetComponent<DoorController>();

                    firefighter.touchDoor = true;
                    //Door interaction
                    hit.transform.gameObject.GetComponent<DoorController>().InteractDoor();

                }

                else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "DoorOutside" && hit.transform.gameObject.GetComponent<DoorController>().containTile(firefighter.getTile()))
                {
                    firefighter.targetDoor = hit.transform.gameObject.GetComponent<DoorController>();

                    firefighter.touchDoor = true;
                    //Door interaction
                    hit.transform.gameObject.GetComponent<DoorController>().InteractDoor();

                }
            }

 //       }

        
        //changing turns
        if (Input.GetKey("0") && Cursor.visible)
        {
            if (Time.time - timePass > keyDelay)
            {
                timePass = Time.time;
                ChangeTurn();
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

    public void TurnFirefighter()
    {

        foreach (FirefighterController fire in firefighters)
        {

            if (fire.myTurn)
            {

                firefighter = fire;

            }

        }
    }

    public FirefighterController getFirefighter()
    {

        return firefighter;

    }
    
    public void ChangeTurn()
    {

        if (firefighter == firefighters[firefighters.Length-1])
        {

            firefighters[firefighters.Length-1].myTurn = false;
            firefighters[0].myTurn = true;

        }
        else for (int i = 0; i < firefighters.Length-1; i++)
        {

            if(firefighter == firefighters[i])
            {

                    firefighters[i].myTurn = false;
                    firefighters[i + 1].myTurn = true;

            }

        }
        
        TurnFirefighter();

    }
    
    public FirefighterController[] getFirefighters()
    {

        return firefighters;

    }

}



