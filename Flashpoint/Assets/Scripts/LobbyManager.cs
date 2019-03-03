using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
   // public static LobbyManager Instance { set; get; }

    private Client client;

    private void Start()
    {
        client = FindObjectOfType<Client>();
    }

    public void ReadyButton()
    {
        string msg = "CRDY|";
        client.Send(msg);//Send ready signal to server
    }
}
