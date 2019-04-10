using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefightePunchWall : MonoBehaviour
{
    private float m_TurnSpeed = 10f;        // Rotate speed

    private Animator m_Animator;            // Reference used to change animation
    private WallController m_TargetWall;    // Reference of the selected Wall
    private Transform m_Transform;          // Reference used to turn the firefighter

    private int m_TargetLife;               // Life of the target wall, used to verify if it has been damaged

    // Set Method
    public void SetTarget(WallController TargetWall)
    {
        m_TargetWall = TargetWall;
        m_TargetLife = m_TargetWall.getLife();
    }

    // Get Method
    public Animator get_m_Animator() { return this.m_Animator; }
    public WallController get_m_TargetWall() { return this.m_TargetWall; }

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    }

    public void Punch()
    {
        m_Animator.SetBool("Punch", true);

        if (m_TargetWall.gameObject.transform.localPosition.x > m_Transform.localPosition.x)
        {
            Turn(90f);
        }

        if (m_TargetWall.gameObject.transform.localPosition.x < m_Transform.localPosition.x)
        {
            Turn(-90f);
        }

        if (m_TargetWall.gameObject.transform.localPosition.z > m_Transform.localPosition.z)
        {
            Turn(0f);
        }

        if (m_TargetWall.gameObject.transform.localPosition.z < m_Transform.localPosition.z)
        {
            Turn(180f);
        }

        StartCoroutine(DamageWall());
    }

    
    IEnumerator DamageWall()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(1f);

        if (!Cursor.visible) { m_TargetWall.gameObject.GetComponent<Wall>().Damage(); }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool TargetDamaged()
    {
        if (m_TargetWall.getLife() < m_TargetLife)
        {
            return true;
        }
        else return false;
    }
    
    public void Turn(float turnInput)
    {
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput, 0f);
        m_Transform.rotation = Quaternion.Lerp(m_Transform.rotation, turnRotation, m_TurnSpeed * Time.deltaTime);
    }

}
