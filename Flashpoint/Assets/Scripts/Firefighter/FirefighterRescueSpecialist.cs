using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterRescueSpecialist : FirefighterManager
{
    private int MovementAP;

    public override void Awake()
    {
        base.Awake();
        MovementAP = 3;
    }

    private void ReduceMovementAP(int ReducedMovementAP)
    {
        MovementAP -= ReducedMovementAP;
    }

    protected override void MoveFirefighter()
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
                    if (MovementAP > 0)
                    {
                        ReduceMovementAP(1);
                    }
                    else
                    {
                        ReduceAP(1);
                    }
                    
                    m_Movement.get_m_Animator().SetBool("Move", false);
                    m_CurrentSpace = m_Movement.get_m_TargetSpace();
                }
            }
            else
            {
                if (AP + MovementAP > 1)
                {
                    if (AP + MovementAP - 2 == 0 && m_Movement.get_m_TargetSpace().status == SpaceStatus.Fire)
                    {
                        Debug.Log("Cannot move to a space on fire at the end of the turn!");
                    }
                    else
                    {
                        m_Movement.Move();
                        if (Vector3.Distance(m_Movement.get_m_Transform().position, m_Movement.get_m_Target()) == 0)
                        {
                            Move = false;
                            if (MovementAP > 1)
                            {
                                ReduceMovementAP(2);
                            }
                            else if(MovementAP > 0)
                            {
                                ReduceMovementAP(1);
                                ReduceAP(1);
                            }
                            else
                            {
                                ReduceAP(2);
                            }
                            
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

    protected override void PunchWall()
    {
        if (!m_PunchWall.TargetDamaged())
            {
                m_PunchWall.Punch();
            }

        // Disable punch wall when it is done
        else
            {
                Punch = false;
                ReduceAP(1);
                m_PunchWall.get_m_Animator().SetBool("Punch", false);
            }        
    }

    protected override void ExtinguishFire()
    {
        if (AP > 1)
        {
            if (!m_Extinguish.FireExtinguished())
            {
                m_Extinguish.Extinguish();
            }
            else
            {
                Extinguish = false;
                m_Extinguish.get_m_Animator().SetBool("Magic", false);
                ReduceAP(2);
            }
        }
        else
        {
            Debug.Log("AP not enough!");
        }
        
    }

    public override void Reset()
    {
        base.Reset();
        this.MovementAP = 3;
    }
}
