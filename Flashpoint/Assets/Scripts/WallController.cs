using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    //action menu variables
    public Canvas myCanvas;
    private bool activeContextMenu = false;
    private string[] chop = { "Chop Wall" };

    //When the mouse hovers over the GameObject, it turns to this color (yellow)
    Color m_MouseOverColor = Color.yellow;

    //This stores the GameObject’s original color
    Color m_OriginalColor1;
    Color m_OriginalColor2;

    MeshRenderer m_Renderer1;
    MeshRenderer m_Renderer2;

    public BoxCollider colli;

    private List<Selectable> neighbours;

    public int life;

    public bool selected = false;
    public string selectedName = "";

    // Start is called before the first frame update
    void Start()
    {

        colli = GetComponent<BoxCollider>();

        neighbours = new List<Selectable>();

        Neighbours();

        life = 2;

        //keeps only the model of the undamanged wall
        foreach (Transform child in transform)
        {

            if (child.CompareTag("BrokenWall"))
            {

                child.gameObject.SetActive(false);
                m_Renderer2 = child.gameObject.GetComponent<MeshRenderer>();
                Color m_OriginalColor2 = m_Renderer2.material.color;

            }
            else
            {

                child.gameObject.SetActive(true);
                m_Renderer1 = child.gameObject.GetComponent<MeshRenderer>();
                Color m_OriginalColor1 = m_Renderer1.material.color;


            }

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

    void OnMouseOver()
    {
        if (Cursor.visible == true && !activeContextMenu && !myCanvas.GetComponent<CanvasManager>().popupOn)
        {
            selected = true;

            foreach (Transform child in transform)
            {

                if (child.transform.parent != null)
                {
                    selectedName = child.transform.parent.name;
                }
                else
                {
                    selectedName = child.name;
                }

                m_Renderer2.material.color = m_MouseOverColor;
                m_Renderer1.material.color = m_MouseOverColor;

            }

            if (Input.GetMouseButtonDown(1))
            {
                //SwitchRendererColor(Color.blue);
                myCanvas.GetComponent<CanvasManager>().ShowActionMenu(chop, this.gameObject, "wall");

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
            SwitchRendererColor(Color.white);
        }
            
    }

    public void SwitchRendererColor(Color myColor)
    {
        foreach (Transform child in transform)
        {

            if (child.CompareTag("BrokenWall") && child.gameObject.activeSelf)
            {
                m_Renderer2.material.color = myColor;
            }
            else if (child.gameObject.activeSelf)
            {
                m_Renderer1.material.color = myColor;
            }

        }
    }


    public void SetActiveContextMenu(bool a)
    {
        activeContextMenu = a;
    }

    void Neighbours()
    {

        Selectable[] tiles = FindObjectsOfType<Selectable>();

        foreach (Selectable tile in tiles)
        {

            if (tile.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {

                if (colli.bounds.Intersects(tile.gameObject.GetComponent<BoxCollider>().bounds))
                {

                    neighbours.Add(tile);

                    tile.addWalls(this);

                }

            }

        }
    }

    //used to see if firefighter is there
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

    public int getLife(){
        return life; 
    }

    public void HitWall()
    {
        life = life - 1;

        if (life == 1)
        {

            //wall broken
            foreach (Transform child in transform)
            {

                if (child.CompareTag("BrokenWall"))
                {

                    child.gameObject.SetActive(true);
                    //m_Renderer2 = child.gameObject.GetComponent<MeshRenderer>();
                    //Color m_OriginalColor2 = m_Renderer2.material.color;

                }
                else
                {

                    child.gameObject.SetActive(false);

                }

            }

        }

        //wall destroyed
        if (life == 0)
        {

            foreach (Transform child in transform)
            {

                if (child.CompareTag("BrokenWall"))
                {

                    child.gameObject.SetActive(false);

                }
                else
                {

                    child.gameObject.SetActive(false);

                }

            }

            foreach (Selectable neigh in neighbours)
            {

                neigh.removeWall(this);

            }

        }

    }

}
