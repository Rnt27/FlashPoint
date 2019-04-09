using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public FirefighterManager m_Firefighter; //Position of the player
    //private FirefighterManager[] m_Firefighters;
    private Vector3 deltaPos;

    // Use this for initialization
    void Start()
    {
        deltaPos = new Vector3(2, 30, -12);
       // m_Firefighters = Game.Instance.m_Firefighters;
        // m_Firefighter = m_Firefighters[0];
        Vector3 m_FirefighterPos = m_Firefighter.transform.TransformDirection(deltaPos);
        transform.position = m_Firefighter.transform.position + m_FirefighterPos;
        Vector3 playerPos = m_Firefighter.transform.position + new Vector3(2, 2, 0);
        transform.LookAt(playerPos);

    }

    void Update()
    {

    }
    /*
    // Update is called once per frame
    void LateUpdate()
    {
        if (m_Firefighters == null)
        {
            m_Firefighters = Game.Instance.m_Firefighters;
        }
        foreach(FirefighterManager firefighter in m_Firefighters)
        {
            if (firefighter.IsMyTurn())
            {
                m_Firefighter = firefighter;
                
            }
        }
    }
    */
}
