using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterTouchDoor : MonoBehaviour
{

    private float m_TurnSpeed = 10f;        // Rotate speed

    private Animator m_Animator;            // Reference used to change animation
    private DoorController m_TargetDoor;    // Reference of the selected Door
    private Transform m_Transform;          // Reference used to turn the firefighter

    // Set Method
    public void SetTarget(DoorController TargetDoor)
    {
        m_TargetDoor = TargetDoor;
    }

    // Get Method
    public Animator get_m_Animator() { return this.m_Animator; }

    // Turn Method: rotate the firefighter when not facing the target door
    public void Turn(float turnInput)
    {
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput, 0f);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, turnRotation, m_TurnSpeed * Time.deltaTime);
    }

    public void TouchDoor()
    {
        m_Animator.SetTrigger("TouchDoor");
        m_TargetDoor.InteractDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
