using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Chat : NetworkBehaviour
{
    public string ChatHistory = "Welcome";
    Vector2 scrollPosition;
    string currentMessage = string.Empty;

    private static Chat callLobbyPlayer;
    public string playerInfo = string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            callLobbyPlayer = this;
            SetupLocalPlayer localPlayer = callLobbyPlayer.GetComponentInParent<SetupLocalPlayer>();
            callLobbyPlayer.playerInfo = localPlayer.pname;
            Debug.Log(playerInfo);

        }
    }

    public void AddMessage(string message)
    {
        callLobbyPlayer.ChatHistory += "\n";
        callLobbyPlayer.ChatHistory += callLobbyPlayer.playerInfo + " : ";
        callLobbyPlayer.ChatHistory += message;
    }

    [ClientRpc]
    public void RpcUpdateChatLog(string chat)
    {
        callLobbyPlayer.ChatHistory = chat;
        Debug.Log("server:" + isServer + "client" + isClient + "LOOOOOL + " + ChatHistory);
    }

    [Command]
    public void CmdUpdateChatLog(string chat)
    {
        callLobbyPlayer.ChatHistory = chat;
        RpcUpdateChatLog(chat);
        Debug.Log("server:" + isServer + "client" + isClient + "keeeeeeeeeeeeeeeeeeek");
    }

    public void OnGUI()
    {
        if (isClient)
        {
            GL.Clear(false, false, Color.black, 0);
            GUILayout.FlexibleSpace();


            {
                scrollPosition = GUILayout.BeginScrollView(
                scrollPosition, GUILayout.Width(300), GUILayout.Height(200));

                GUILayout.Label(ChatHistory);

                GUILayout.EndScrollView();
            }

            currentMessage = GUILayout.TextField(currentMessage);
            if (GUILayout.Button("Send"))
            {
                AddMessage(currentMessage);
                callLobbyPlayer.CmdUpdateChatLog(callLobbyPlayer.ChatHistory);
                currentMessage = null;
            };
        }
    }
}
