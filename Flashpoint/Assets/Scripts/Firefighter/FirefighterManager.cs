using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirefighterManager : MonoBehaviour
{
    //public Transform m_SpawnPoint;                          // The position and direction the firefighter will have when it spawns
    //public int m_PlayerNumber;                              // This specifies which player this firefighter belongs to
    //public GameObject m_Instance;                           // A reference to the instance of the firefighter when it is created

    private int AP;                                           // The action points firefighter has    
    private bool myTurn;                                      // This specifies if it is this firefighter's turn, controlled by FirefighterManager
    private bool isCarryingVictim;

    private Selectable currentTile;


    // Boolean variables to control access to actions
    private bool Move;
    private bool Punch;
    private bool TouchDoor;

    // Methods enable actions, called by GameManager
    public void EnableMove() { Move = true; }
    public void EnablePunch() { Punch = true; }
    public void EnableTouchDoor() { TouchDoor = true; }

    // References to firefighter's action scripts
    private FirefighterMovement m_Movement;
    private FirefightePunchWall m_PunchWall;
    private FirefighterTouchDoor m_TouchDoor;

    // Methods set action target
    public void SetTargetTile(Selectable TargetTile) { m_Movement.SetTarget(TargetTile); }
    public void SetTargetWall(WallController TargetWall) { m_PunchWall.SetTarget(TargetWall); }

    public void Awake ()
    {
        m_Movement = GetComponent<FirefighterMovement> ();
        m_PunchWall = GetComponent<FirefightePunchWall>();

        AP = 4;
        Move = false;
        Punch = false;
        
        //m_Movement.m_PlayerNumber = m_PlayerNumber;
    }

    void Update()
    {
        // Move
        if (Move)
        {
            m_Movement.Move();

            // Disable movement action when it is done;
            if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
            {
                Move = false;
                currentTile = m_Movement.get_m_TargetTile();
                m_Movement.get_m_Animator().SetBool("Move", false);
            }
        }

        // Punch
        if (Punch)
        {
            m_PunchWall.Punch();

            if (m_PunchWall.TargetDamaged())
            {
                Punch = false;
                m_PunchWall.get_m_Animator().SetBool("Punch", false);
            }
        }
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_PunchWall.enabled = false;
    }

    public void EnableControl()
    {
        m_Movement.enabled = true; ;
        m_PunchWall.enabled = true;
    }

    

    public void Reset()
    {

    }
}
