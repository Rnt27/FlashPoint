using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameClient activePlayer;

    List<GameClient> players;

    Client game;

    FirefighterController[] firefighters;

    // Start is called before the first frame update
    void Start()
    {
        /*game.GetComponents<Client>();

        players = game.getPlayers();

        activePlayer = players[0];

        firefighters = FindObjectsOfType<FirefighterController>();
        */
        //firefighters[0].myTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
