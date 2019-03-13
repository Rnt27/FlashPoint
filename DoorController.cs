using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public BoxCollider collider;

    public bool open;

    int life;

    private Transform pivot;

    private GameObject door;

    //To get neighbours
    public List<Selectable> neighbours;
    
    //When the mouse hovers over the GameObject, it turns to this color (yellow)
    Color m_MouseOverColor = new Color(255,255,0,0);

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    public bool selected = false;
    public string selectedName = "";

    Quaternion rota = new Quaternion();

    FirefighterController firefighter;

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

    }

    void closeD()
    {

            foreach (Transform child in transform)
            {
                if (child.CompareTag("Door"))
                {
                    child.transform.RotateAround(pivot.position, Vector3.down, 75);
                }
            }

            open = false;

    }

    void openD()
    {
        foreach (Transform child in transform)
            {
                if (child.CompareTag("Door"))
                {
                    child.transform.RotateAround(pivot.position, Vector3.down, -75);
                }
            }

            open = true;

    }

    //how to interact with the door(open/close)
    public void interactDoor()
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
                        
        }
    }

    void OnMouseExit()
    {
        selected = false;

        selectedName = "";
        // Reset the color of the GameObject back to normal

        foreach (Transform child in transform)
        {

            if (child.CompareTag("Clickable") && child.gameObject.activeSelf)
            {
                m_Renderer.material.color = m_OriginalColor;
            }
            
        }
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
}
