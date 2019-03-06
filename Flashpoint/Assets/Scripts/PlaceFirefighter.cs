using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFirefighter : MonoBehaviour{

    public GameObject firefighter;
    public GameObject placeFirefighterPanel;
    public GameObject mainGamePanel;
    private GameObject tileHighlight;
    


    private void Start()
    {
        
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        firefighter = Instantiate(firefighter, point, Quaternion.identity, gameObject.transform);
        firefighter.SetActive(false);
    }
    

    void Update()
    {
        MouseOverLocation();

        //Place firefighter only if he has not yet done so
        if (Input.GetMouseButtonDown(0) && !firefighter.activeSelf) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector2Int gridPoint = Geometry.GridFromPoint(hit.point);
                if (Between(gridPoint.x,-14, 25, true) && Between(gridPoint.y, -11, 21, true)) // within larger box
                {
                    // not within smaller box
                    if (!(Between(gridPoint.x, -10, 21, true) && Between(gridPoint.y, -7, 17, true)))
                    {
                        firefighter.SetActive(true);
                        placeFirefighterPanel.SetActive(false);
                        firefighter.transform.position = Geometry.PointFromGrid(centerOfTile(gridPoint));
                        return;
                    }
                }
            }
        }
    }

    private Vector2Int centerOfTile(Vector2Int pos)
    {
        Debug.Log("clicked:" + pos);
        Vector2Int newPos = new Vector2Int();
        // Find the index of the square
        int i = (pos.x + 14) / 4;
        Debug.Log("index:" + i);
        newPos.x = i*4 - 14 + 2; // somewhat centered

        newPos.y = (int)pos.y / 4;
        newPos.y *= 4;
        return newPos;
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



