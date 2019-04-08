using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterGeneralist : FirefighterManager
{
    string m_name = "Generalist";
    public FirefighterGeneralist()
    {
        AP = 5;
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
