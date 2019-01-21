using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFirefighter : MonoBehaviour{

    public GameObject firefighter;
    public GameObject placeFirefighterPanel;
    public GameObject mainGamePanel;
    private GameObject tileHighlight;

    /*
    public GameObject[,] startTiles;

    public GameObject EightZero;
    public GameObject SevenZero;
    public GameObject SixZero;
    public GameObject FiveZero;
    public GameObject FourZero;
    public GameObject ThreeZero;
    public GameObject TwoZero;
    public GameObject OneZero;

    public GameObject ZeroOne;
    public GameObject ZeroTwo;
    public GameObject ZeroThree;
    public GameObject ZeroFour;
    public GameObject ZeroFive;
    public GameObject ZeroSix;

    public GameObject OneSeven;
    public GameObject TwoSeven;
    public GameObject ThreeSeven;
    public GameObject FourSeven;
    public GameObject FiveSeven;
    public GameObject SixSeven;
    public GameObject SevenSeven;
    public GameObject EightSeven;

    public GameObject NineSix;
    public GameObject NineFive;
    public GameObject NineFour;
    public GameObject NineThree;
    public GameObject NineTwo;
    public GameObject NineOne;
    */



    private void Start()
    {
        //startTiles = new GameObject[10, 8];
        //PopulateStartTiles();

        //launchPad = gameObject.GetComponent<Transform>();
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        firefighter = Instantiate(firefighter, point, Quaternion.identity, gameObject.transform);
        firefighter.SetActive(false);
    }
    

    void Update()
    {   
        //Place firefighter only if he has not yet done so
        if (Input.GetMouseButtonDown(0) && !firefighter.activeSelf) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                Vector2Int gridPoint = Geometry.GridFromPoint(point);
                Debug.Log(gridPoint);
                if (Between(gridPoint.x, -14, -10, true) || Between(gridPoint.x, 21, 25, true))
                {
                    if (Between(gridPoint.y, -6, 17, true))
                    {
                        firefighter.SetActive(true);
                        placeFirefighterPanel.SetActive(false);
                        firefighter.transform.position = Geometry.PointFromGrid(gridPoint);

                    }
                }
                if ( Between(gridPoint.y, -11, -9, true) || Between(gridPoint.y, 17, 21, true))
                {
                    if (Between(gridPoint.x, -10, 21, true))
                    {
                        firefighter.SetActive(true);
                        placeFirefighterPanel.SetActive(false);
                        firefighter.transform.position = Geometry.PointFromGrid(gridPoint);

                    }
                }
            }
        }
    }

    public static bool Between( int num, int lower, int upper, bool inclusive = false)
    {
        return inclusive
            ? lower <= num && num <= upper
            : lower < num && num < upper;
    }

    /*
    public void AddPiece(GameObject obj, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        startTiles[col, row] =
            Instantiate(obj, Geometry.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform);
    }

    private void PopulateStartTiles()
    {
        AddPiece(EightZero, 8, 0);
        AddPiece(SevenZero, 7, 0);
        AddPiece(SixZero, 6, 0);
        AddPiece(FiveZero, 5, 0);
        AddPiece(FourZero, 4, 0);
        AddPiece(ThreeZero, 3, 0);
        AddPiece(TwoZero, 2, 0);
        AddPiece(OneZero, 1, 0);

        AddPiece(ZeroOne, 0, 1);
        AddPiece(ZeroTwo, 0, 2);
        AddPiece(ZeroThree, 0, 3);
        AddPiece(ZeroFour, 0, 4);
        AddPiece(ZeroFive, 0, 5);
        AddPiece(ZeroSix, 0, 6);

        AddPiece(OneSeven, 1, 7);
        AddPiece(TwoSeven, 2, 7);
        AddPiece(ThreeSeven, 3, 7);
        AddPiece(FourSeven, 4, 7);
        AddPiece(FiveSeven, 5, 7);
        AddPiece(SixSeven, 6, 7);
        AddPiece(SevenSeven, 7, 7);
        AddPiece(EightSeven, 8, 7);

        AddPiece(NineSix, 9, 6);
        AddPiece(NineFive, 9, 5);
        AddPiece(NineFour, 9, 4);
        AddPiece(NineThree, 9, 3);
        AddPiece(NineTwo, 9, 2);
        AddPiece(NineOne, 9, 1);
    }
    */
}
//public void PlaceFirefighter()
//{
//    public Vector2 mousePos  = Input.mousePosition;

//    //Depth you want the center of the object to be is z which I used zero
//    public Vector3 boxPos = camera.ScreenToWorldPoint(mousePos.x, mousePos.y, 0);

//    //I used the perfab box's rotation here but you can enter what you'd like as a euler using Quaternion.Euler(x,y,z)
//    Instantiate(firefighter, boxPos, firefighter.transform.rotation);
//}


