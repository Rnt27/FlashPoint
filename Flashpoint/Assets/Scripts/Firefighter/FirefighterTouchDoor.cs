using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterTouchDoor : MonoBehaviour
{

    private float m_TurnSpeed = 10f;        // Rotate speed

    private Animator m_Animator;            // Reference used to change animation
    private DoorController m_TargetDoor;    // Reference of the selected Door
    private Transform m_Transform;          // Reference used to turn the firefighter

    private bool m_DoorState;               // open or not open

    // Set Method
    public void SetTarget(DoorController TargetDoor)
    {
        m_TargetDoor = TargetDoor;
        m_DoorState = m_TargetDoor.getDoorState();
    }

    // Get Method
    public Animator get_m_Animator() { return this.m_Animator; }

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    }

    // Turn Method: rotate the firefighter when not facing the target door
    public void Turn(float turnInput)
    {
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput, 0f);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, turnRotation, m_TurnSpeed * Time.deltaTime);
    }

    // Action
    public void TouchDoor()
    {
        m_Animator.SetTrigger("TouchDoor");
        if (m_TargetDoor.gameObject.transform.localPosition.x > m_Transform.localPosition.x)
        {
            Turn(90f);
        }

        if (m_TargetDoor.gameObject.transform.localPosition.x < m_Transform.localPosition.x)
        {
            Turn(-90f);
        }

        if (m_TargetDoor.gameObject.transform.localPosition.z > m_Transform.localPosition.z)
        {
            Turn(0f);
        }

        if (m_TargetDoor.gameObject.transform.localPosition.z < m_Transform.localPosition.z)
        {
            Turn(180f);
        }
        m_TargetDoor.InteractDoor();
        //StartCoroutine(TouchDoorRoutine()); ;
    }

    IEnumerator TouchDoorRoutine()
    {
        m_Animator.SetTrigger("TouchDoor");
        yield return new WaitForSeconds(2f);
        m_TargetDoor.InteractDoor();
    }

    public bool DoorTouched()
    {
        if (m_DoorState != m_TargetDoor.getDoorState())
        {
            return true;
        }
        else return false;
    }
}
