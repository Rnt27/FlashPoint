using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/* Edit by Steven Wang
 * 
 */

public class Game : MonoBehaviour
{

    //public CameraView m_CameraControl;
    //public GameObject m_FirefighterPrefab;        // Reference to the prefab the players will control

    public FirefighterManager[] m_Firefighters;     // A collection of managers for enabling and disabling different aspects of the Firefighter
    //public FirefighterManager firefighter;

    private bool m_hasLevelStarted = false;
    private bool m_isGamePlaying = false;
    private bool m_isGameOver = false;
    private bool m_hasLevelFinished = false;

    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

    private WaitForSeconds m_StartWait = new WaitForSeconds(1f);
    private WaitForSeconds m_EndWait = new WaitForSeconds(1f);

    void Awake()
    {
        m_Firefighters = FindObjectsOfType<FirefighterManager>();
        //firefighter = m_Firefighters[0];
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundStarting()
    {
        DisableFirefighterControl();
        yield return m_StartWait;
    }

    private IEnumerator RoundPlaying()
    {
        m_isGamePlaying = true;
        while (!m_isGameOver)
        {
            foreach (FirefighterManager firefighter in m_Firefighters)
            {
                firefighter.EnableAction();
                firefighter.EnableControl();

                yield return StartCoroutine(TurnPlaying(firefighter));
                firefighter.Reset();
            }

            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        Debug.Log("Firefighter Disabled");
        DisableFirefighterControl();
        yield return m_EndWait;
    }

    private IEnumerator TurnPlaying(FirefighterManager firefighter)
    {
        while (firefighter.getAP() != 0 && firefighter.IsMyTurn())
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
            Debug.Log("Firefighter No." + firefighter.m_PlayerNumber + " AP: " + firefighter.getAP());
            yield return null;
        }
    }


    /*
    void Update()
    {
       

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
    
    
    */

    private void DisableFirefighterControl()
    {
        foreach(FirefighterManager firefighter in m_Firefighters)
        {
            firefighter.DisableControl();
        }
    }

    private void EnableFirefighterControl()
    {
        foreach (FirefighterManager firefighter in m_Firefighters)
        {
            firefighter.EnableControl();
        }
    }
}
