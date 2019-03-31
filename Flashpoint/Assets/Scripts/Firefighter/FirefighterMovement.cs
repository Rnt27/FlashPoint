using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterMovement : MonoBehaviour
{
    //public int m_PlayerNumber = 1;        // Used to identify which firefighter belongs to which player. Set by firefighter's manager
    public float m_Speed = 2.0f;            // Move speed
    public float m_TurnSpeed = 180f;        // Rotate speed

    private Transform m_Transform;          // Reference used to move the firefighter
    private Vector3 m_Target;               // Reference of the target location
    private Animator m_Animator;            // Reference used to change animation

    private Selectable m_TargetTile;        // Reference of the selected tile

    // Get methods
    public Transform get_m_Transform() { return this.m_Transform; }
    public Vector3 get_m_Target() { return this.m_Target; }
    public Animator get_m_Animator() { return this.m_Animator; }

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    }

    public void SetTarget(Selectable TargetTile)
    {
        this.m_TargetTile = TargetTile;
        this.m_Target = m_TargetTile.transform.position + new Vector3(0, 1, 0);
    }

    public void Move()
    {
        m_Animator.SetBool("Move", true);
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, m_Target, m_Speed * Time.deltaTime);
    }

    public void Turn(float turnInput)
    {
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput, 0f);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, turnRotation, 3 * Time.deltaTime);
    }
}
