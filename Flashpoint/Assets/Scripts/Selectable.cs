using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selectable : MonoBehaviour
{

    public BoxCollider collider;

    // action menu variable
    private bool isConcrete = false;
    public Canvas myCanvas;
    private bool activeContextMenu = false;
    private string[] ActionSpaceSafe = { "Move to here", "Carry victim to here" };
    private string[] ActionSpaceFire = { "Move to here", "Carry victim to here", "Turn Fire to Smoke", "Extinguish Fire" };
    private string[] ActionSpaceSmoke = { "Move to here", "Carry victim to here", "Extinguish Smoke" };
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
        if (gameObject.name == "Concrete")
        {
            isConcrete = true;
        }
        //Debug.Log("mycanvas= " + myCanvas);
        CanvasManager myCanvasManager = myCanvas.GetComponent<CanvasManager>();
        //Debug.Log("mycanvasManager= " + myCanvasManager);
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

    public void SwitchColorToOriginal()
    {
        m_Renderer.material.color = m_OriginalColor;

    }

    public void SetActiveContextMenu(bool a)
    {
        activeContextMenu = a;
    }
    


    void OnMouseOver()
    {
        
        if (Cursor.visible==true && !activeContextMenu && !myCanvas.GetComponent<CanvasManager>().popupOn)
        {

            selected = true;
            if (this.transform.parent != null)
            {
                selectedName = this.transform.parent.name;
            }
            else
            {
                selectedName = this.name;
            }
            //Debug.Log("Renderer =  " + m_Renderer);

            // Change the color of the GameObject to yellow when the mouse is over GameObject
            m_Renderer.material.color = m_MouseOverColor;

            if (Input.GetMouseButtonDown(1) && !isConcrete)
            {
                //m_Renderer.material.color = Color.blue;
                if(gameObject.GetComponent<Space>().status == SpaceStatus.Safe)
                {
                    myCanvas.GetComponent<CanvasManager>().ShowActionMenu(ActionSpaceSafe, this.gameObject, "tile");
                }

                else if (gameObject.GetComponent<Space>().status == SpaceStatus.Fire)
                {
                    myCanvas.GetComponent<CanvasManager>().ShowActionMenu(ActionSpaceFire, this.gameObject, "tile");
                }

                else if (gameObject.GetComponent<Space>().status == SpaceStatus.Smoke)
                {
                    myCanvas.GetComponent<CanvasManager>().ShowActionMenu(ActionSpaceSmoke, this.gameObject, "tile");
                }


                
            }




        }
    }

    void OnMouseExit()
    {
        selected = false;

        selectedName = "";
        // Reset the color of the GameObject back to normal
        if (!activeContextMenu)
        {
            m_Renderer.material.color = m_OriginalColor;
        }
        
        
        
        
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
