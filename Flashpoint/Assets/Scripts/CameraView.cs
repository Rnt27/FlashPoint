using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CameraView : MonoBehaviour {
    public Transform Player; //Position of the player
    private Vector3 deltaPos; //Position of the camera

    public GameObject target;
    //public float followAhead;

    private Vector3 targetPosition;

    public float smoothing = 2;

    public FirefighterController firefighter;

    private Vector3 originalPlace;
    private Quaternion originalRotation;

    FirefighterController[] controlFirefighter;

    //FirefighterController[] firefighters;

    bool camera0;
    bool camera1;
    bool camera2;
    bool camera3;
    bool camera4;

    private bool setup = false;

    

    public void Setup()
    {



        controlFirefighter = FindObjectsOfType<FirefighterController>();
        Debug.Log("camera= " + controlFirefighter.Length);
        //Debug.Log(firefighters[1].name);
        foreach (FirefighterController f in controlFirefighter)
        {
            if (f.name == "local")
            {
                firefighter = f;

            }
        }
        
    }

    // called third
    // Use this for initialization
    void Start () {

        //target = FindObjectOfType<FirefighterController>().gameObject;

        //controlFirefighter = FindObjectOfType<FirefighterController>();

        //firefighters = controlFirefighter.getFirefighters();

        //firefighter = controlFirefighter.firefighter;
        //firefighter = controlFirefighter;

        //firefighters[0].myTurn = true;

        //turnFirefighter();

        //firefighter = FindObjectOfType<FirefighterController>();
        /*        deltaPos = new Vector3(2, 30, -12);
                    Vector3 pos = Player.TransformDirection(deltaPos);
                    transform.position = Player.position + pos;
                    Vector3 playerPos = Player.position + new Vector3(2, 2, 0);
                    transform.LookAt(playerPos);
                    */
        originalPlace = transform.position;

        originalRotation = transform.rotation;

        camera0 = false;

        camera1 = false;

        camera2 = false;

        camera3 = false;

        camera4 = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!setup)
        {
            Setup();
        }
        target = firefighter.gameObject;
        


        //turnFirefighter();

        //controlFirefighter.turnFirefighter();

        //firefighter = controlFirefighter;


        //Debug.Log(target.name);

        //Inputs to change the camera movement
        if (Input.GetKey("`"))
        {

            camera0 = true;

            camera1 = false;

            camera2 = false;

            camera3 = false;

            camera4 = false;
        }

        if (firefighter.spawned == false && !camera0 && !camera1 && !camera2 && !camera3 && !camera4)
        {

            transform.position = Vector3.Lerp(transform.position, originalPlace, smoothing * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, smoothing * Time.deltaTime);

        }

        if (camera0 && !camera1 && !camera2 && !camera3 && !camera4)
        {

            transform.position = Vector3.Lerp(transform.position, originalPlace, smoothing * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, smoothing * Time.deltaTime);

        }

        if (firefighter.spawned==true && !camera0 && !camera1 && !camera2 && !camera3 && !camera4)
        {

            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 16, target.transform.position.z - 10);
                        
            Quaternion rota = new Quaternion();

            rota = Quaternion.Euler(45,0,0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smoothing * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); //changing camera smoothly, where camera is, where camera needs to be, how much time to be there
                                                                                                               //Time.deltaTime is so that things happen the same for fast and slow computers no matter the speed
        }

        if (Input.GetKeyUp("1") && firefighter.spawned == true)
        {

            camera0 = false;

            camera1 = true;

            camera2 = false;

            camera3 = false;

            camera4 = false;
        }

        if (!camera0 && camera1 && !camera2 && !camera3 && !camera4)
        {

            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 16, target.transform.position.z - 10);

            Quaternion rota = new Quaternion();

            rota = Quaternion.Euler(45, 0, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smoothing * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); //changing camera smoothly, where camera is, where camera needs to be, how much time to be there
                                                                                                               //Time.deltaTime is so that things happen the same for fast and slow computers no matter the speed
        }

        if (Input.GetKeyUp("2") && firefighter.spawned == true)
        {

            camera0 = false;

            camera1 = false;

            camera2 = true;

            camera3 = false;

            camera4 = false;
        }

        if (!camera0 && !camera1 && camera2 && !camera3 && !camera4)
        {

            targetPosition = new Vector3(target.transform.position.x-12, target.transform.position.y + 16, target.transform.position.z);

            Quaternion rota = new Quaternion();

            rota = Quaternion.Euler(45, 90, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smoothing * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); //changing camera smoothly, where camera is, where camera needs to be, how much time to be there
                                                                                                               //Time.deltaTime is so that things happen the same for fast and slow computers no matter the speed
        }

        if (Input.GetKeyUp("3") && firefighter.spawned == true)
        {

            camera0 = false;

            camera1 = false;

            camera2 = false;

            camera3 = true;

            camera4 = false;
        }

        if (!camera0 && !camera1 && !camera2 && camera3 && !camera4)
        {

            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 16, target.transform.position.z + 10);

            Quaternion rota = new Quaternion();

            rota = Quaternion.Euler(45, -180, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smoothing * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); //changing camera smoothly, where camera is, where camera needs to be, how much time to be there
                                                                                                               //Time.deltaTime is so that things happen the same for fast and slow computers no matter the speed
        }

        if (Input.GetKeyUp("4") && firefighter.spawned == true)
        {

            camera0 = false;

            camera1 = false;

            camera2 = false;

            camera3 = false;

            camera4 = true;
        }

        if (!camera0 && !camera1 && !camera2 && !camera3 && camera4)
        {

            targetPosition = new Vector3(target.transform.position.x +12, target.transform.position.y + 16, target.transform.position.z + 1);

            Quaternion rota = new Quaternion();

            rota = Quaternion.Euler(45, -90, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rota, smoothing * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); //changing camera smoothly, where camera is, where camera needs to be, how much time to be there
                                                                                                               //Time.deltaTime is so that things happen the same for fast and slow computers no matter the speed
        }

    }
/*
    private void turnFirefighter()
    {

        foreach (FirefighterController fire in firefighters)
        {

            if (fire.myTurn)
            {

                firefighter = fire;

            }

        }
    }
  */  
}
