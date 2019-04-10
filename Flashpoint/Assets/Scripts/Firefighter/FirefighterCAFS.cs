using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterCAFS : FirefighterManager
{
    private int ExtinguishAP;

    public override void Awake()
    {
        base.Awake();
        ExtinguishAP = 3;
    }

    private void ReduceExtinguishAP(int reducedExtinguishAP)
    {
        ExtinguishAP -= reducedExtinguishAP; 
    }

    protected override void ExtinguishFire()
    {
        if (!m_Extinguish.FireExtinguished())
        {
            m_Extinguish.Extinguish();
        }
        else
        {
            Extinguish = false;
            m_Extinguish.get_m_Animator().SetBool("Magic", false);
            if (ExtinguishAP > 0)
            {
                ReduceExtinguishAP(1);
            }
            else
            {
                ReduceAP(1);
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        this.ExtinguishAP = 3;
    }
}
