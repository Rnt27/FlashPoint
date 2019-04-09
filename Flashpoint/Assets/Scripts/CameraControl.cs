using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject gameBoard;
    public FirefighterManager[] m_Firefighters;

    public Vector3 offset;
    private Vector3 deltaPos;

    // Use this for initialization
    void Start()
    {
        deltaPos = new Vector3(2, 30, -10);
        StartCoroutine(findTargets());
        StartCoroutine(UpdateCamera());
    }
    
    private IEnumerator UpdateCamera()
    {
        //Debug.Log("Game over: " + Game.Instance.IsGameOver);
        while (!Game.Instance.IsGameOver)
        {
            if (Game.Instance.IsTurnPlaying && Game.Instance.FirefighterAllSpawned())
            {
                foreach (GameObject firefighter in targets)
                {
                    if (firefighter.GetComponent<FirefighterManager>().IsMyTurn())
                    {
                        // Debug.Log("Targets found!");
                        deltaPos = new Vector3(2, 15, -5);
                        Vector3 pos = firefighter.transform.TransformDirection(deltaPos);
                        transform.position = firefighter.transform.position + pos;
                        Vector3 playerPos = firefighter.transform.position + new Vector3(2, 2, 0);
                        transform.LookAt(playerPos);


                        // Vector3 centerPoint = firefighter.transform.position;
                        // Vector3 newPosition = centerPoint + offset;
                        // transform.position = newPosition;
                    }
                }
            }
            else
            {
                Vector3 pos = gameBoard.transform.TransformDirection(deltaPos);
                transform.position = gameBoard.transform.position + pos;
                Vector3 playerPos = gameBoard.transform.position + new Vector3(2, 2, 0);
                transform.LookAt(playerPos);
            }
            yield return null;
        }
    }
    
    private IEnumerator findTargets()
    {

        yield return new WaitForSeconds(2f);
        m_Firefighters = FindObjectsOfType<FirefighterManager>();
        foreach (FirefighterManager firefighter in m_Firefighters)
        {
            targets.Add(firefighter.gameObject);
            Debug.Log("Target Added!");
        }
    }
}
