using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    private Client client;

    public GameObject mainMenu; //Menu on UI
    public GameObject serverMenu; //Host on UI
    public GameObject connectMenu; //ConnectMenu on UI

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public InputField nameInput;

    public Transform playersConnectedContainer;
    public GameObject playerNamePrefab;

    void Start()
    {
        Instance = this;
        client = FindObjectOfType<Client>();

        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void MenuConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    public void MenuHostButton()
    {
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            c.isHost = true;

            if (c.clientName == "")
                c.clientName = "Fireman Host";
            c.ConnectToServer("127.0.0.1", 6321);
        }
        catch (Exception e)
        {
            Debug.Log("MenuHostButton exception " + e.Message);
        }

        mainMenu.SetActive(false);
        serverMenu.SetActive(true);


    }

    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;//Get text on HostInput input field

        if (hostAddress == null)
            hostAddress = "127.0.0.1";

        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Fireman Client";
            c.ConnectToServer(hostAddress, 6321);
        }
        catch (Exception e)
        {
            Debug.Log("ConnectToServer Error" + e.Message);
            connectMenu.SetActive(false);
        }
    }
    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);

        //must destroy server
        Server s = FindObjectOfType<Server>();
        if (s != null)
            Destroy(s.gameObject);

        //must destroy client
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(c.gameObject);

        //reset list of connected players
        ResetUserConnectedPanel();
    }
    public void ResetUserConnectedPanel()
    {
        foreach (Transform child in playersConnectedContainer)
        {
            Destroy(child.gameObject);
        }
    }
    public void StartLobby()
    {
        SceneManager.LoadScene("LobbyFamily");
    }
    public void StartFFPlacement()
    {
        Debug.Log("FirefighterPosition");

        SceneManager.LoadScene("FirefighterPosition");
    }
    
    public void DisplayUserConnected(string name)
    {
        GameObject go = Instantiate(playerNamePrefab) as GameObject;
        go.transform.SetParent(playersConnectedContainer);
        go.GetComponentInChildren<Text>().text = name;
        go.transform.localScale = new Vector3(1, 1, 1);


    }
}
