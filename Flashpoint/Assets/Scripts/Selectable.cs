﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : MonoBehaviour
{

    public BoxCollider collider;

    // action menu variable
    public GameObject actionPrefab;
    public Transform actionContainer;
    private bool no_menu = true;
    private string[] action = { "Move to here", "Carry victim to here" };
    private string[] extinguishAction = {"Extinguish Smoke", "Extinguish Fire"};
    private string chop = "Chop Wall";
    private string[] doorAction = { "Open Door", "Close Door"};
    private string paramedicAction = "Treat"; // Extinguish Fire = 4 AP  and Extinguish Smoke =  2AP
    private string imagingTechnicianAction = "Identify POI";
    private string hazmatTechnicianAction = "Dispose";

    //for blinking
    public float wait;

    //When the mouse hovers over the GameObject, it turns to this color (yellow)
    Color m_MouseOverColor = Color.yellow;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    //To get neighbours
    private List<Selectable> neighbours;
    public List<WallController> walls;
    public List<DoorController> doors;

    public bool selected = false;
    public string selectedName = "";
    // Start is called before the first frame update
    void Start()
    {
        //cam = GetComponent<Camera>();
        //Fetch the mesh renderer component from the GameObject
        m_Renderer = GetComponent<MeshRenderer>();
        //Fetch the original color of the GameObject
        m_OriginalColor = m_Renderer.material.color;

        collider = GetComponent<BoxCollider>();

        neighbours = new List<Selectable>();

        walls = new List<WallController>();

        doors = new List<DoorController>();

        Neighbours();
    }
        
    // Update is called once per frame
    void Update()
    {
        

    }

    void actionMenu(string[] actions)
    {
        //no_menu = !no_menu;
        m_Renderer.material.color = Color.blue;

        //find the context menu
        Canvas myCanvas = FindObjectOfType<Canvas>();
        GameObject contextMenu = myCanvas.transform.Find("ContextMenu").gameObject;
        contextMenu.SetActive(true);
        Vector2 menuPosition = Input.mousePosition;
        contextMenu.transform.position = new Vector3(menuPosition.x + 35, menuPosition.y - 65, 0);
        //contextMenu.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        //GameObject panel = contextMenu.transform.Find("actionPanel").gameObject;

        foreach (Transform child in actionContainer)
        {
            //GameObject a = child as GameObject;
            GameObject.Destroy(child.gameObject);
            // do whatever you want with each child transform here
        }

        foreach (string action in actions)
        {
            //Debug.Log(action);

            GameObject go = Instantiate(actionPrefab) as GameObject;
            go.transform.SetParent(actionContainer);
            go.GetComponentInChildren<Text>().text = action;
            Debug.Log("child: " + go.transform.GetChild(0).transform.GetChild(1));
            int ap = 1; //evaluate AP needed
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ap.ToString() + "AP";
            go.transform.localScale = new Vector3(1, 1, 1);

        }

    }

    void OnMouseOver()
    {
        if(Cursor.visible==true){

            selected = true;
            if (this.transform.parent != null)
            {
                selectedName = this.transform.parent.name;
            }
            else
            {
                selectedName = this.name;
            }

            // Change the color of the GameObject to yellow when the mouse is over GameObject
            m_Renderer.material.color = m_MouseOverColor;

            if (Input.GetMouseButtonDown(1))
            {

                actionMenu(action);
            }




        }
    }

    void OnMouseExit()
    {
        selected = false;

        selectedName = "";
        // Reset the color of the GameObject back to normal
        m_Renderer.material.color = m_OriginalColor;
    }

    void Neighbours()
    {

        Selectable[] tiles = FindObjectsOfType<Selectable>();

        foreach (Selectable tile in tiles)
        {

            if(tile.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {

                if(collider.bounds.Intersects(tile.gameObject.GetComponent<BoxCollider>().bounds))
                {

                    neighbours.Add(tile);

                }

            }

        }
    }

    public bool isAdjacent(Selectable tile)
    {

        if (neighbours.Contains(tile))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void addWalls(WallController wall)
    {

        walls.Add(wall);

    }

    public void addDoors(DoorController door)
    {

        doors.Add(door);

    }

    public void removeWall(WallController wall)
    {

        walls.Remove(wall);

    }

    public void removeDoor(DoorController door)
    {

        doors.Remove(door);

    }

    //check if tile has wall
    public bool containWall(WallController wall)
    {

        if (walls.Contains(wall))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //Check if tile has door
    public bool containDoor(DoorController door)
    {

        if (doors.Contains(door))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //Returns true if tile contains closed door
    public bool containsClosed(DoorController door)
    {

        if (doors.Contains(door))
        {

            if (!door.open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    //checks if both tiles have the same wall
    public bool compareWalls(Selectable tile)
    {

        foreach(WallController wall in walls)
        {

            if (tile.containWall(wall))
            {
                return true;
            }

        }
        return false;

    }

    //checks if both tiles have the same door
    public bool compareDoors(Selectable tile)
    {

        foreach (DoorController door in doors)
        {

            if (tile.containDoor(door))
            {
                return true;
            }

        }
        return false;

    }
    
    //Compares if two tiles are between a closed door
    public bool compareClose(Selectable tile)
    {

        foreach (DoorController door in doors)
        {

            if (tile.containsClosed(door))
            {
                return true;
            }

        }
        return false;

    }

    /*public DoorController getDoor(Selectable tile)
    {



    }*/
    /*private IEnumerator Blink()
    {

        m_Renderer.material.color = m_MouseOverColor;
        yield return new WaitForSeconds(wait);
        m_Renderer.material.color = m_OriginalColor;

    }*/
}
