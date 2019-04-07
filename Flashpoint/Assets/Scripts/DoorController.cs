using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // action menu variables
    public Canvas myCanvas;
    private bool activeContextMenu = false;
    private string[] doorAction = { "Open Door", "Close Door" };


    public BoxCollider collider;

    public bool open;

    int life;

    public float smooth;

    private Transform pivot;

    private GameObject door;

    //To get neighbours
    public List<Selectable> neighbours;

    //When the mouse hovers over the GameObject, it turns to this color (yellow)
    Color m_MouseOverColor = new Color(255, 255, 0, 0);

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    public bool selected = false;
    public string selectedName = "";

    Quaternion rota = new Quaternion();

    FirefighterController firefighter;

    Transform doorAnimation;

    // Start is called before the first frame update
    void Start()
    {

        open = false;

        life = 1;

        collider = GetComponent<BoxCollider>();

        //To help door rotate
        pivot = this.transform.Find("Concrete (1)");

        neighbours = new List<Selectable>();

        Neighbours();

        foreach (Transform child in transform)
        {

            if (child.CompareTag("Clickable"))
            {

                //Fetch the mesh renderer component from the GameObject
                m_Renderer = child.gameObject.GetComponent<MeshRenderer>();
                //Fetch the original color of the GameObject
                m_OriginalColor = m_Renderer.material.color;

                //collider = child.GetComponent<BoxCollider>();

            }

            if (child.CompareTag("GhostAnimation"))
            {

                doorAnimation = child;

            }

        }

        if (this.gameObject.CompareTag("DoorOutside"))
        {

            openD();

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (life == 0)
        {

            this.gameObject.SetActive(false);
        }

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Door"))
            {
                child.position = Vector3.Lerp(child.transform.position, doorAnimation.transform.position, 0.01F * Time.time);
                child.rotation = Quaternion.Lerp(child.transform.rotation, doorAnimation.transform.rotation, 0.01F * Time.time);
            }

        }

    }

    void closeD()
    {

        doorAnimation.transform.RotateAround(pivot.position, Vector3.down, 75);

        open = false;

    }

    void openD()
    {
        doorAnimation.transform.RotateAround(pivot.position, Vector3.down, -75);

        open = true;

    }

    //how to interact with the door(open/close)
    public void InteractDoor()
    {
        if (!open)
        {

            openD();

        }
        else
        {

            closeD();

        }
    }

    void OnMouseOver()
    {
        if (Cursor.visible == true)
            selected = true;
        {

            foreach (Transform child in transform)
            {

                if (child.CompareTag("Clickable"))
                {

                    if (child.transform.parent != null)
                    {
                        selectedName = child.transform.parent.name;
                    }
                    else
                    {
                        selectedName = child.name;
                    }

                    m_Renderer.material.color = m_MouseOverColor;

                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //SwitchRendererColor(Color.blue);
                myCanvas.GetComponent<CanvasManager>().ShowActionMenu(doorAction, this.gameObject, "door");

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
            SwitchRendererColor();
        }

    }

    public void SwitchRendererColor()
    {

        foreach (Transform child in transform)
        {

            if (child.CompareTag("Clickable") && child.gameObject.activeSelf)
            {
                m_Renderer.material.color = m_OriginalColor;
            }

        }

    }

    public void SetActiveContextMenu(bool a)
    {
        activeContextMenu = a;
    }

    //to damage the door
    public void damageDoor()
    {

        life = 0;

    }

    void Neighbours()
    {

        Selectable[] tiles = FindObjectsOfType<Selectable>();

        foreach (Selectable tile in tiles)
        {

            if (tile.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {

                if (collider.bounds.Intersects(tile.gameObject.GetComponent<BoxCollider>().bounds))
                {

                    neighbours.Add(tile);

                    tile.addDoors(this);
                }

            }

        }
    }

    public bool containTile(Selectable tile)
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
    
    public bool getDoorState()
    {
        return open;
    }
    /*
    public bool containClosed(Selectable tile)
    {

        if (neighbours.Contains(tile))
        {
            if (!this.open)
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
    */
}