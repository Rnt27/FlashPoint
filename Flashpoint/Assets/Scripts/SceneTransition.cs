using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneTransition : MonoBehaviour {
    public GameObject LBM;

    private void Start()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        if (CheckSceneOK( sceneName))
        {
            Application.LoadLevel(sceneName);
        }
       
    }

    bool CheckSceneOK(string sceneName)
    {
        if (sceneName == "StartGame")
        {
            Application.Quit();
            //Destroy(NetworkLobbyManager.singleton.gameObject);
            
           // NetworkManager.Shutdown();

           // Application.LoadLevel("sceneName);
          //  Instantiate(NetworkLobbyManager.singleton.gameObject);
            //Instantiate(NetworkManager.singleton.gameObject);

            //Application.Quit();
            //NetworkManager.singleton.StopMatchMaker();


            // NetworkLobbyManager.singleton.StopClient();
            // NetworkLobbyManager.singleton.StopServer();

            //NetworkServer.DisconnectAll();
            // Application.LoadLevel(sceneName);
            //  Debug.Log("disconnect ");

            //   StartCoroutine(ExitDelay(sceneName));

            return false;

        }
        return true;
    }

    IEnumerator ExitDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
      //  Destroy(NetworkLobbyManager.singleton.gameObject);

        yield return new WaitForSeconds(0.1f);
        //NetworkServer.Reset();
       
      //  NetworkManager.singleton.ServerChangeScene(sceneName);
      //  Debug.Log("disconnect ");

    }
}
