using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class MyLobbyHook : LobbyHook
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //gamePlayer.GetComponent<Renderer>().material.color = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();

            localPlayer.pname = lobby.playerName;
            localPlayer.playerColor = lobby.playerColor;
        }
    }

}

