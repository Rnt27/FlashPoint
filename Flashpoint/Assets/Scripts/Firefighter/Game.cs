using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Edit by Steven Wang
 * 
 */

    [System.Serializable]
public class Game : MonoBehaviour
{
	public static Game Instance = null; 
    public FirefighterManager[] m_Firefighters;     // A collection of managers for enabling and disabling different aspects of the Firefighter
    public FirefighterManager m_Firefighter;
    public int nTurn = 0; //count turn number

    private bool m_hasLevelStarted = false;
    private bool m_isGamePlaying = false;
    private bool m_isGameOver = false;
    private bool m_hasLevelFinished = false;
    private bool m_isTurnPlaying = false;
    private bool m_isEndTurnPlaying = false;

    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }
    public bool IsTurnPlaying { get { return m_isTurnPlaying; } set { m_isTurnPlaying = value; } }
    public bool IsEndTurnPlaying { get { return m_isEndTurnPlaying; } set { m_isEndTurnPlaying = value; } }

    private bool moveButtonActive = false;
    private bool punchButtonActive = false;
    private bool touchButtonActive = false;
    private bool extinguishButtonActive = false;
    private bool endTurnButtonActive = false;

    public bool GetMoveButtonState() { return moveButtonActive; }
    public bool GetPunchButtonState() { return punchButtonActive; }
    public bool GetTouchButtonState() { return touchButtonActive; }
    public bool GetExtinguishButtonState() { return extinguishButtonActive; }
    public bool GetEndTurnButtonState() { return endTurnButtonActive; }

    public void SetMoveButtonState(bool b) { this.moveButtonActive = b; }
    public void SetPunchButtonState(bool b) { this.punchButtonActive = b; }
    public void SetTouchButtonState(bool b) { this.touchButtonActive = b; }
    public void SetExtinguishButtonState(bool b) { this.extinguishButtonActive = b; }
    public void SetEndTurnButtonState(bool b) { this.endTurnButtonActive = b; }

    public void setMoveButtonActive() { this.moveButtonActive = true; }
    public void setPunchButtonActive() { this.punchButtonActive = true; }
    public void setTouchButtonActive() { this.touchButtonActive = true; }
    public void setExtinguishButtonActive() { this.extinguishButtonActive = true; }
    public void setEndTurnButtonActive() { this.endTurnButtonActive = true; }

    private WaitForSeconds m_StartWait = new WaitForSeconds(5f);
    private WaitForSeconds m_EndWait = new WaitForSeconds(5f);

    public List<GameObject> GetFFOnSpace(GameObject target)
    {
	    List<GameObject> onSpace = new List<GameObject>();
	    Space targetSpace = target.GetComponent<Space>();
	    foreach (FirefighterManager f in m_Firefighters) //Check each firefighter and see if their current space is the space
	    {
			if(f.getCurrentSpace() == targetSpace) onSpace.Add(f.gameObject);
	    }

	    return onSpace;
    }


    void Awake()
    {
	    if (Instance == null)
	    {
		    Instance = this;
	    }
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
        yield return m_StartWait;
        m_Firefighters = FindObjectsOfType<FirefighterManager>();
        Debug.Log("Firefighter number: " + m_Firefighters.Length);
        DisableFirefighterControl();
        while (!FirefighterAllSpawned())
        {
            foreach (FirefighterManager firefighter in m_Firefighters)
            {
                firefighter.EnableAction();
                yield return StartCoroutine(SpawnFirefighter(firefighter));
                firefighter.DisableAction();
            }
            yield return null;
        }
        // yield return m_StartWait;
    }

    private IEnumerator RoundPlaying()
    {
        nTurn++;
        Debug.Log(nTurn);
        m_isGamePlaying = true;
        while (!m_isGameOver)
        {
            foreach (FirefighterManager firefighter in m_Firefighters)
            {
                firefighter.EnableAction();
                firefighter.EnableControl();

                yield return StartCoroutine(TurnPlaying(firefighter));
                firefighter.Reset();
                m_isTurnPlaying = false;
                endTurnButtonActive = false;

                yield return StartCoroutine(AdvanceFire());
                m_isEndTurnPlaying = false;
                
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

    private IEnumerator SpawnFirefighter(FirefighterManager firefighter)
    {
        while (!firefighter.isSpawned)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "OutsideTile")
                {
                    firefighter.Spawn(hit.transform.gameObject.GetComponent<Space>());
                    firefighter.isSpawned = true;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator TurnPlaying(FirefighterManager firefighter)
    {
        m_isTurnPlaying = true; 
        while (firefighter.getAP() > 0 && firefighter.IsMyTurn() && !endTurnButtonActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
               
                if (Physics.Raycast(ray, out hit) && firefighter.getCurrentSpace().IsAdjacent(hit.transform.gameObject))
                {
                    Debug.Log("The space is adjacent:" + firefighter.getCurrentSpace().IsAdjacent(hit.transform.gameObject));
                    if ((hit.transform.gameObject.tag == "InsideTile" || hit.transform.gameObject.tag == "OutsideTile"))
                    {
                        
                        firefighter.SetTargetSpace(hit.transform.gameObject.GetComponent<Space>());
                        firefighter.EnableMove();
                        string msg = "Player No." + firefighter.m_PlayerNumber+1 + "'s Firefighter Moved to a new space " ;
                        this.gameObject.GetComponent<gameMsg>().ShowMessage(msg);
                    }

                    else if (hit.transform.gameObject.tag == "Wall")
                    {
                        
                        firefighter.SetTargetWall(hit.transform.gameObject.GetComponent<WallController>());
                        firefighter.EnablePunch();
                        string msg = "Player No." + firefighter.m_PlayerNumber+1 + "'s Firefighter Punched a wall ";
                        this.gameObject.GetComponent<gameMsg>().ShowMessage(msg);
                    }

                    else if ((hit.transform.gameObject.tag == "DoorInside" || hit.transform.gameObject.tag == "DoorOutside"))
                    {
                        
                        firefighter.SetTargetDoor(hit.transform.gameObject.GetComponent<DoorController>());
                        firefighter.EnableTouchDoor();
                        string msg = "Player No." + firefighter.m_PlayerNumber+1 + "'s Firefighter changed a door state ";
                        this.gameObject.GetComponent<gameMsg>().ShowMessage(msg);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "InsideTile" && hit.transform.gameObject.GetComponent<Space>().status != SpaceStatus.Safe)
                {
                    firefighter.SetTargetFire(hit.transform.gameObject.GetComponent<Space>());
                    firefighter.EnableExtinguish();
                }
            }
            //Debug.Log("Firefighter No." + firefighter.m_PlayerNumber + " AP: " + firefighter.getAP());
            yield return null;
        }
    }


    IEnumerator AdvanceFire()
    {
        m_isEndTurnPlaying = true;
        BoardManager.Instance.EndTurn();
        yield return new WaitForSeconds(5f);
    }


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

    public bool FirefighterAllSpawned()
    {
        bool AllSpawned = true;
        foreach (FirefighterManager firefighter in m_Firefighters)
        {
            if (!firefighter.isSpawned) 
            {
                AllSpawned = false;
            }
        }
        return AllSpawned;
    }
}
