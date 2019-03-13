using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	//main menu buttons
	
    public Button loadGameButton;
    public Button joinGameButton;
    public Button exitGameButton;

	//scenes
	public GameObject saveGamesList;


	public void Awake() {
		//create for all functions w corresponding button
		loadGameButton.onClick.AddListener(LoadSavedFiles);
        //Join game is performs onClick()->run Lobbies
        exitGameButton.onClick.AddListener(ExitGame);

	}


	public void LoadSavedFiles(){
		saveGamesList.SetActive(true);
	}


    public void ExitGame(){
		Debug.Log("trying to exit game");
		Application.Quit();
	}


}
