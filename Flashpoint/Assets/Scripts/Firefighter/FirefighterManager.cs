using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FirefighterManager : MonoBehaviour
{
    [HideInInspector] public int m_PlayerNumber;              // This specifies which player this firefighter belongs to
    [HideInInspector] public GameObject m_Instance;           // A reference to the instance of the firefighter when it is created
    [HideInInspector] public bool isSpawned;

    protected Space m_CurrentSpace;
    protected Animator m_Animator;

    protected int AP;                                           // The action points firefighter has    
    protected int savedAP;                                      // The action points firefighter saved
    protected bool myTurn;                                      // This specifies if it is this firefighter's turn, controlled by FirefighterManager
    protected bool isCarryingVictim;                            // This specifies if the firefighter is carrying a victim
	protected Victim victim;

    // Boolean variables to control access to actions
    protected bool Move;
    protected bool Punch;
    protected bool TouchDoor;
    protected bool Extinguish;

    // Methods enable actions, called by GameManager
    public void EnableAction() { myTurn = true; }
    public void EnableMove() { Move = true; }
    public void EnablePunch() { Punch = true; }
    public void EnableTouchDoor() { TouchDoor = true; }
    public void EnableExtinguish() { Extinguish = true; }

    // Methods disable turn
    public void DisableAction() { myTurn = false; }

    // References to firefighter's action scripts
    protected FirefighterMovement m_Movement;
    protected FirefightePunchWall m_PunchWall;
    protected FirefighterTouchDoor m_TouchDoor;
    protected FirefighterExtinguish m_Extinguish;

    // Methods set action target
    public void SetTargetSpace(Space TargetSpace) { m_Movement.SetTarget(TargetSpace); }
    public void SetTargetWall(WallController TargetWall) { m_PunchWall.SetTarget(TargetWall); }
    public void SetTargetDoor(DoorController TargetDoor) { m_TouchDoor.SetTarget(TargetDoor); }
    public void SetTargetFire(Space TargetSpace) { m_Extinguish.SetTarget(TargetSpace); }

    // Set Methods
    public void setCurrentSpace(Space TargetSpace) { this.m_CurrentSpace = TargetSpace; }
    public void setAP(int a)
	{
		this.AP = a;
		APManager.Instance.setAP(AP);
		Debug.Log("New AP: " + AP);
	}
    public void ReduceAP(int reducedAP)
	{
		setAP(AP - reducedAP);
	}


    public void setIsCarryingVictim(bool b) { isCarryingVictim = b; }


    // Get Methods
    public int getAP() { return this.AP; }
    public bool IsMyTurn() { return this.myTurn; }
    public bool IsCarryingVictim() { return this.isCarryingVictim; }
    public Space getCurrentSpace() { return this.m_CurrentSpace; }

    // Disable all action control at the end of player's turn, called in GameManager
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_PunchWall.enabled = false;
        m_TouchDoor.enabled = false;
        m_Extinguish.enabled = false;
    }

    // Enable all action control at the beginning of player's turn, called in GamaManager
    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_PunchWall.enabled = true;
        m_TouchDoor.enabled = true;
        m_Extinguish.enabled = true;

        setAP(AP + savedAP);
        savedAP = 0;
    }

    // Awake
    public virtual void Awake()
    {
        m_Movement = GetComponent<FirefighterMovement>();
        m_PunchWall = GetComponent<FirefightePunchWall>();
        m_TouchDoor = GetComponent<FirefighterTouchDoor>();
        m_Extinguish = GetComponent<FirefighterExtinguish>();
        m_Animator = GetComponent<Animator>();

		if(isCarryingVictim == false)
		{
			victim = null;
		}

        isSpawned = false;

        setAP(4);
        savedAP = 0;
        myTurn = false;

        Move = false;
        Punch = false;
        TouchDoor = false;
        Extinguish = false;

        //m_Movement.m_PlayerNumber = m_PlayerNumber;
    }

    

    public void Spawn(Space SpawnSpace)
    {
        transform.position = SpawnSpace.transform.position + new Vector3(0,1,0);
        m_Animator.SetTrigger("Spawn");
        m_CurrentSpace = SpawnSpace;
    }

    // Update
    protected virtual void Update()
    {
        if (myTurn)
        {
            if (Move) { MoveFirefighter(); }
            if (Punch) { PunchWall(); }
            if (TouchDoor) { InteractDoor(); }
            if (Extinguish) { ExtinguishFire(); }
        }
    }

    protected virtual void MoveFirefighter()
    {
        if (m_Movement.get_m_TargetSpace().status == SpaceStatus.Fire && isCarryingVictim)
        {
            Debug.Log("Unable to move to a space with fire when carrying victim!");
        }
        else
        {
            if (m_Movement.get_m_TargetSpace().status == SpaceStatus.Safe && !isCarryingVictim)
            {
                m_Movement.Move();
                if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
                {
                    Move = false;
                    ReduceAP(1);
                    m_Movement.get_m_Animator().SetBool("Move", false);
                    m_CurrentSpace = m_Movement.get_m_TargetSpace();
                }
            }
            else
            {
                if (AP > 1)
                {
                    if (AP - 2 == 0 && m_Movement.get_m_TargetSpace().status == SpaceStatus.Fire)
                    {
                        Debug.Log("Cannot move to a space on fire at the end of the turn!");
                    }
                    else
                    {
                        m_Movement.Move();
                        if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
                        {
                            Move = false;
                            ReduceAP(2);
                            m_Movement.get_m_Animator().SetBool("Move", false);
                            m_CurrentSpace = m_Movement.get_m_TargetSpace();
                        }
                    }   
                }
                else
                {
                    Debug.Log("AP not enough!");
                }
            }
        }
    }

    protected virtual void PunchWall()
    {
        if (AP > 1)
        {
            if (!m_PunchWall.TargetDamaged())
            {
                m_PunchWall.Punch();
            }

            // Disable punch wall when it is done
            else
            {
                Punch = false;
                ReduceAP(2);
                m_PunchWall.get_m_Animator().SetBool("Punch", false);
            }
        }
        else
        {
            Debug.Log("AP not enough!");
        }
    }

    protected virtual void InteractDoor()
    {
        m_TouchDoor.TouchDoor();

        // Disable touch door when it is done
        if (m_TouchDoor.DoorTouched())
        {
            TouchDoor = false;
            ReduceAP(1);
        }
    }

    protected virtual void ExtinguishFire()
    {
        if (!m_Extinguish.FireExtinguished())
        {
            m_Extinguish.Extinguish();
        }
        else
        {
            Extinguish = false;
            m_Extinguish.get_m_Animator().SetBool("Magic", false);
            ReduceAP(1);
        }
    }

    public virtual void Reset()
    {
        if(AP != 0)
        {
            if (savedAP + AP >= 4)
            {
                savedAP = 4;
            }
            else
            {
                savedAP = savedAP + AP;
            }
        }
        this.myTurn = false;
        setAP(4);
    }
}
