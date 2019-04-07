using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FirefighterManager : MonoBehaviour
{
    [HideInInspector] public int m_PlayerNumber;              // This specifies which player this firefighter belongs to
    [HideInInspector] public GameObject m_Instance;           // A reference to the instance of the firefighter when it is created

    private Selectable currentTile;

    private int AP;                                           // The action points firefighter has    
    private bool myTurn;                                      // This specifies if it is this firefighter's turn, controlled by FirefighterManager
    private bool isCarryingVictim;  

    // Boolean variables to control access to actions
    private bool Move;
    private bool Punch;
    private bool TouchDoor;
    private bool Extinguish;

    // Methods enable actions, called by GameManager
    public void EnableAction() { myTurn = true; }
    public void EnableMove() { Move = true; }
    public void EnablePunch() { Punch = true; }
    public void EnableTouchDoor() { TouchDoor = true; }
    public void EnableExitinguish() { Extinguish = true; }

    // References to firefighter's action scripts
    private FirefighterMovement m_Movement;
    private FirefightePunchWall m_PunchWall;
    private FirefighterTouchDoor m_TouchDoor;

    // Methods set action target
    public void SetTargetTile(Selectable TargetTile) { m_Movement.SetTarget(TargetTile); }
    public void SetTargetWall(WallController TargetWall) { m_PunchWall.SetTarget(TargetWall); }
    public void SetTargetDoor(DoorController TargetDoor) { m_TouchDoor.SetTarget(TargetDoor); }
    
    // Get Methods
    public int getAP() { return this.AP; }
    public bool IsMyTurn() { return this.myTurn; }
    public bool IsCarryingVictim() { return this.isCarryingVictim; }

    // Awake
    public void Awake()
    {
        m_Movement = GetComponent<FirefighterMovement>();
        m_PunchWall = GetComponent<FirefightePunchWall>();
        m_TouchDoor = GetComponent<FirefighterTouchDoor>();

        AP = 4;
        myTurn = false;

        Move = false;
        Punch = false;
        TouchDoor = false;
        Extinguish = false;

        //m_Movement.m_PlayerNumber = m_PlayerNumber;
    }

    // Setup
    /*
    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<FirefighterMovement>();
        m_PunchWall = m_Instance.GetComponent<FirefightePunchWall>();
        m_TouchDoor = m_Instance.GetComponent<FirefighterTouchDoor>();
    }
    */

    // Disable all action control at the end of player's turn, called in GameManager
    public void DisableControl()
    {
            m_Movement.enabled = false;
            m_PunchWall.enabled = false;
            m_TouchDoor.enabled = false;
    }

    // Enable all action control at the beginning of player's turn, called in GamaManager
    public void EnableControl()
    {
        // Debug.Log("Firefighter Enabled");
        m_Movement.enabled = true;
        m_PunchWall.enabled = true;
        m_TouchDoor.enabled = true;
    }

    public void ReduceAP(int reducedAP)
    {
        AP -= reducedAP;
    }

    

    // Update
    void Update()
    {
        if (myTurn)
        {
            // Move
            if (Move)
            {
                m_Movement.Move();

                // Disable movement action when it is done
                if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
                {
                    Move = false;
                    ReduceAP(1);
                    //Debug.Log(AP.ToString());
                    //currentTile = m_Movement.get_m_TargetTile();
                    m_Movement.get_m_Animator().SetBool("Move", false);
                }
            }

            // Punch
            if (Punch)
            {
                m_PunchWall.Punch();

                // Disable punch wall when it is done
                if (m_PunchWall.TargetDamaged())
                {
                    Punch = false;
                    ReduceAP(1);
                    m_PunchWall.get_m_Animator().SetBool("Punch", false);
                }
            }

            // TouchDoor
            if (TouchDoor)
            {
                m_TouchDoor.TouchDoor();

                // Disable touch door when it is done
                if (m_TouchDoor.DoorTouched())
                {
                    TouchDoor = false;
                    ReduceAP(1);
                    // m_TouchDoor.get_m_Animator().SetBool("TouchDoor", false);
                }
            }
 
            // Extinguish

            // Carry
        }
    }    

    public void Reset()
    {
        this.myTurn = false;
        this.AP = 4;
    }
}
