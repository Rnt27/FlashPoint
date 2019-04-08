using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPOI : MonoBehaviour
{
   
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "POI")
        {
            col.transform.parent = gameObject.transform;
            Debug.Log("Collided: " + col.transform.name);

        }

        if (col.gameObject.tag == "FalseAlarm")
        {
            Destroy(col.gameObject);

        }


    }


}
