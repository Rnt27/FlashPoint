using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
    [SyncVar]
    public string pname = "firefigther";

    [SyncVar]
    public Color playerColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Renderer>().material.color = playerColor;
        if (isLocalPlayer)
        {  
            this.name = "local";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
