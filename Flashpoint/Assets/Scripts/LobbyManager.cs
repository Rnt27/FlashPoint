using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // public static LobbyManager Instance { set; get; }

    private Client client;

    public GameObject showPlayersButton;
    public GameObject showPlayersPanel; //Menu on UI
    public GameObject playerNamePrefab; 
    public Transform playersConnectedContainer;


    private void Start()
    {
        client = FindObjectOfType<Client>();
    }

    public void ReadyButton()
    {
        string msg = "CRDY|";
        client.Send(msg);//Send ready signal to server
    }

    public void ShowPlayers()
    {
        List<GameClient> players = client.getPlayers();
        showPlayersButton.SetActive(false);
        showPlayersPanel.SetActive(true);
        foreach (GameClient c in players)
        {
            Debug.Log(c.name);

            GameObject go = Instantiate(playerNamePrefab) as GameObject;
            go.transform.SetParent(playersConnectedContainer);
            go.GetComponentInChildren<Text>().text = c.name;
            go.transform.localScale = new Vector3(1, 1, 1);

        }
    }

}
