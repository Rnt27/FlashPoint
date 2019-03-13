using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Transform Player;

	// Use this for initialization
	void Start () {
        Player = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Player.Translate(Vector3.forward * 4);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Player.Rotate(Vector3.up * -90);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Player.Rotate(Vector3.up * 90);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Player.Rotate(Vector3.up * 180);
        }
    }
}
