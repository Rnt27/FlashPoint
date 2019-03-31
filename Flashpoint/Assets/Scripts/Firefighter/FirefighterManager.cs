using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirefighterManager : MonoBehaviour
{
    //public Transform m_SpawnPoint;                          // The position and direction the firefighter will have when it spawns
    //public int m_PlayerNumber;                              // This specifies which player this firefighter belongs to
    //public GameObject m_Instance;                           // A reference to the instance of the firefighter when it is created

    private int AP;                                         // The action points firefighter has    
    private bool myTurn;                                    // This specifies if it is this firefighter's turn, controlled by FirefighterManager
    private bool isCarryingVictim;
    private bool isMoving;
    private FirefighterMovement m_Movement;                 // Reference to firefighter's movement script, used to disable and enable control

    public void Awake ()
    {
        m_Movement = GetComponent<FirefighterMovement> ();

        isMoving = false;

        //m_Movement.m_PlayerNumber = m_PlayerNumber;

    }

    void Start()
    {
        
    }

    void Update()
    {
        //m_Movement.Move();
    if (isMoving)
         {
             m_Movement.Move();

             if(Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
             {
                 isMoving = false;
                 m_Movement.get_m_Animator().SetBool("Move", false);
             }
         }
    }

    public void EnableMove()
    {
        isMoving = true;
    }

    public void DisableControl()
    {
        m_Movement.enabled = false;
    }

    public void EnableControl()
    {
        m_Movement.enabled = true; ;
    }

    public void SetTargetTile(Selectable TargetTile)
    {
        m_Movement.SetTarget(TargetTile);
    }

    public void Reset()
    {

    }



}
