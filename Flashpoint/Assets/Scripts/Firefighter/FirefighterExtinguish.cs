﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterExtinguish : MonoBehaviour
{
    private float m_TurnSpeed = 10f;        // Rotate speed

    private Animator m_Animator;            // Reference used to change animation
    private Space m_TargetSpace;            // Reference of the selected space which has fire on it
    private Transform m_Transform;          // Reference used to turn the firefighter

    public Transform get_m_Transform() { return this.m_Transform; }
    public Animator get_m_Animator() { return this.m_Animator; }
    public Space get_m_TargetSpace() { return this.m_TargetSpace; }

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    }

    public void SetTarget(Space TargetSpace)
    {
        this.m_TargetSpace = TargetSpace;
    }

    public void Extinguish()
    {
        m_Animator.SetBool("Magic", true);
        if (m_TargetSpace.gameObject.transform.localPosition.x > m_Transform.localPosition.x)
        {
            Turn(90f);
        }
        if (m_TargetSpace.gameObject.transform.localPosition.x < m_Transform.localPosition.x)
        {
            Turn(-90f);
        }
        if (m_TargetSpace.gameObject.transform.localPosition.z > m_Transform.localPosition.z)
        {
            Turn(0f);
        }
        if (m_TargetSpace.gameObject.transform.localPosition.z < m_Transform.localPosition.z)
        {
            Turn(180f);
        }
        m_TargetSpace.DecrementFire();
    }

    public void Turn(float turnInput)
    {
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput, 0f);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, turnRotation, m_TurnSpeed * Time.deltaTime);
    }
}
