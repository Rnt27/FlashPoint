using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterRescueDog : FirefighterManager
{
    public override void Awake()
    {
        m_Movement = GetComponent<FirefighterMovement>();
        m_PunchWall = GetComponent<FirefightePunchWall>();
        m_TouchDoor = GetComponent<FirefighterTouchDoor>();
        m_Extinguish = GetComponent<FirefighterExtinguish>();
        m_Animator = GetComponent<Animator>();

        isSpawned = false;

        AP = 12;
        savedAP = 0;
        myTurn = false;

        Move = false;
        Punch = false;
        TouchDoor = false;
        Extinguish = false;
    }

    protected override void MoveFirefighter()
    {
        if (m_Movement.get_m_TargetSpace().status == SpaceStatus.Fire)
        {
            Debug.Log("Rescue Dog cannot move to a space on fire!");
        }
        else
        {
            if (!isCarryingVictim )
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
                if (AP > 3)
                { 
                    m_Movement.Move();
                    if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
                    {

                        Move = false;
                        ReduceAP(4);
                        m_Movement.get_m_Animator().SetBool("Move", false);
                        m_CurrentSpace = m_Movement.get_m_TargetSpace();
                     }
                    
                }
                else
                {
                    Debug.Log("AP not enough!");
                }
            }
        }
    }
    protected override void PunchWall()
    {
        if (m_PunchWall.get_m_TargetWall().getLife() == 1 && !isCarryingVictim)
        {
            m_Animator.SetBool("Move", true);
            transform.position = Vector3.MoveTowards(transform.position, m_PunchWall.get_m_TargetWall().transform.position, 5f * Time.deltaTime);

            if (Vector3.Distance(m_PunchWall.get_m_TargetWall().transform.position, transform.position) == 0)
            {
                Punch = false;
                ReduceAP(2);
                m_Movement.get_m_Animator().SetBool("Move", false);
            }
        }
    }
    protected override void InteractDoor()
    {
        Debug.Log("Rescue Dog cannot open/close doors!");
        return;
    }
    protected override void ExtinguishFire()
    {
        Debug.Log("Rescue Dog cannot extinguish fire/smoke!");
        return;
    }

}
