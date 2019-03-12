using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{

    public BoxCollider collider;

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

        Neighbours();
    }
        
    // Update is called once per frame
    void Update()
    {
        
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

            // Change the color of the GameObject to black when the mouse is over GameObject
            m_Renderer.material.color = m_MouseOverColor;


            if (Input.GetMouseButtonDown(1))
            {
                //find the context menu
                GameObject contextMenu = GameObject.FindWithTag("contextMenu");
                contextMenu.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);

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

    public void removeWall(WallController wall)
    {

        walls.Remove(wall);

    }

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
    /*private IEnumerator Blink()
    {

        m_Renderer.material.color = m_MouseOverColor;
        yield return new WaitForSeconds(wait);
        m_Renderer.material.color = m_OriginalColor;

    }*/
}
