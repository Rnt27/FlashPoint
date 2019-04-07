using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Edit by Steven Wang
 * 
 */

public class Game : MonoBehaviour
{

    //public CameraView m_CameraControl;
    //public GameObject m_FirefighterPrefab;          // Reference to the prefab the players will control
    public FirefighterManager[] m_Firefighters;     // A collection of managers for enabling and disabling different aspects of the Firefighter
    public FirefighterManager firefighter;
    // Start is called before the first frame update

    private WaitForSeconds m_StartWait = new WaitForSeconds(1f);
    private WaitForSeconds m_EndWait = new WaitForSeconds(1f);


    void Start()
    {
        
        //Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        //Vector3 point = Geometry.PointFromGrid(gridPoint);
        firefighter = FindObjectOfType<FirefighterManager>();
        //StartCoroutine(GameLoop());
    }

    //void Update()
    //{
    //    SpawnAllFirefighters();
    //}
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && (hit.transform.gameObject.tag == "InsideTile" || hit.transform.gameObject.tag == "OutsideTile"))
            {
                firefighter.SetTargetTile(hit.transform.gameObject.GetComponent<Selectable>());
                firefighter.EnableMove();
            }

            else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Wall")
            {
                firefighter.SetTargetWall(hit.transform.gameObject.GetComponent<WallController>());
                firefighter.EnablePunch();
            }

            else if (Physics.Raycast(ray, out hit) && (hit.transform.gameObject.tag == "DoorInside" || hit.transform.gameObject.tag == "DoorOutside"))
            {
                firefighter.SetTargetDoor(hit.transform.gameObject.GetComponent<DoorController>());
                firefighter.EnableTouchDoor();
            }
        }

        //while (firefighter.getAP() > 0)
        //{

        //}
    }
    
    
    /*
    private void SpawnAllFirefighters()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        for (int i = 0; i < m_Firefighters.Length; i++)
        {
            Transform current_firefighter_transform;
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "OutsideTile")
                {
                    current_firefighter_transform = hit.transform.gameObject.GetComponent<Selectable>().transform;
                    m_Firefighters[i].m_Instance = Instantiate(m_FirefighterPrefab, current_firefighter_transform) as GameObject;
                }
            }
            
            //m_Firefighters[i].m_PlayerNumber = i + 1;
           // m_Firefighters[i].Setup();
        }
    }
    */
    /*
    
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        //yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {
        DisableFirefighterControl();
        yield return m_StartWait;
    }
    
    private IEnumerator RoundPlaying()
    {
        EnableFirefighterControl();
       
        
            yield return null;
        
    }
    */
    /*
    private IEnumerator RoundEnding()
    {
        DisableFirefighterControl();
        yield return m_EndWait;
    }
    */

    private void DisableFirefighterControl()
    {
        firefighter.DisableControl();
    }

    private void EnableFirefighterControl()
    {
        firefighter.EnableControl();
    }
}
