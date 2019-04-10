using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterGeneralist : FirefighterManager
{
    string m_name = "Generalist";
    
    public override void Awake()
    {
        m_Movement = GetComponent<FirefighterMovement>();
        m_PunchWall = GetComponent<FirefightePunchWall>();
        m_TouchDoor = GetComponent<FirefighterTouchDoor>();
        m_Extinguish = GetComponent<FirefighterExtinguish>();
        m_Animator = GetComponent<Animator>();

        isSpawned = false;

        AP = 5;
        savedAP = 0;
        myTurn = false;

        Move = false;
        Punch = false;
        TouchDoor = false;
        Extinguish = false;
    }

    public override void Reset()
    {
        if (AP != 0)
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
        this.AP = 5;
    }
}
