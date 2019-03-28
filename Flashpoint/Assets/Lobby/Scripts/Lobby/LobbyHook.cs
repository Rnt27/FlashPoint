using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public class LobbyHook : MonoBehaviour
    {
        public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
            //gamePlayer.GetComponent<Renderer>().material.color = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();

            localPlayer.pname = lobby.playerName;
            localPlayer.playerColor = lobby.playerColor;
        }
    }

}
