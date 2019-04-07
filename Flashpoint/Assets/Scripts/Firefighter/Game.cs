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
    //public FirefighterManager[] m_Firefighters;     // A collection of managers for enabling and disabling different aspects of the Firefighter
    public FirefighterManager firefighter;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnAllFirefighters();
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        firefighter = FindObjectOfType<FirefighterManager>();
        //StartCoroutine(GameLoop());
    }

    void Update()
    {
        //MouseOverLocation();

        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && (hit.transform.gameObject.tag == "InsideTile"  || hit.transform.gameObject.tag == "OutsideTile"))
            {
                firefighter.SetTargetTile(hit.transform.gameObject.GetComponent<Selectable>());
                firefighter.EnableMove();
            }

            else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Wall")
            {
                firefighter.SetTargetWall(hit.transform.gameObject.GetComponent<WallController>());
                firefighter.EnablePunch();
            }
        }*/


        
        

    }

    //Test action menu
    public void Move(Selectable target)
    {
        firefighter.SetTargetTile(target);
        firefighter.EnableMove();

    }

    public void Chop(WallController target)
    {
        firefighter.SetTargetWall(target);
        firefighter.EnablePunch();

    }

    /*
    private void SpawnAllFirefighters()
    {
        for(int i = 0; i < m_Firefighters.Length; i++)
        {
            m_Firefighters[i].m_Instance = Instantiate(m_FirefighterPrefab, m_Firefighters[i].m_SpawnPoint.position, m_Firefighters[i].m_SpawnPoint.rotation) as GameObject;
            m_Firefighters[i].m_PlayerNumber = i + 1;
            m_Firefighters[i].Setup();
        }
    }
    
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {

    }

    private IEnumerator RoundPlaying()
    {

    }

    private IEnumerator RoundEnding()
    {

    }
    */
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
}
