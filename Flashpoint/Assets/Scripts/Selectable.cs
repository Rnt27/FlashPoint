using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{

    
    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = Color.black;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

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
    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
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

        // Change the color of the GameObject to black when the mouse is over GameObject
        m_Renderer.material.color = m_MouseOverColor;

        if (Input.GetMouseButtonDown(1))
        {
            //find the context menu
            GameObject contextMenu = GameObject.FindWithTag("contextMenu");
            contextMenu.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);

        }
    }

    void OnMouseExit()
    {
        selected = false;

        selectedName = "";
        // Reset the color of the GameObject back to normal
        m_Renderer.material.color = m_OriginalColor;
    }
}
