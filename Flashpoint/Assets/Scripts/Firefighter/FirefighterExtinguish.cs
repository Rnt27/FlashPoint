using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterExtinguish : MonoBehaviour
{
    private float m_TurnSpeed = 10f;        // Rotate speed

    private Animator m_Animator;            // Reference used to change animation
    private Selectable m_TargetFire;    // Reference of the selected Wall
    private Transform m_Transform;          // Reference used to turn the firefighter

    private int m_TargetLife;               // Life of the target wall, used to verify if it has been damaged

    // Update is called once per frame
    void Update()
    {
        
    }
}
