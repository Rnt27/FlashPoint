using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APManager : MonoBehaviour
{
    public GameObject AP1;
    public GameObject AP2;
    public GameObject AP3;
    public GameObject AP4;
    public GameObject AP5;

    public GameObject SP1;
    public GameObject SP2;
    public GameObject SP3;

    public GameObject VAP;

    bool veteran = false;

    public RectTransform savedTransform;
    private float cachedY;

    private float minX;
    private float maxX;

    public int currentSaved;

    private int maxSaved;
    public Text savedText;

    public Text APnow;
    public Text SPnow;

    public Image visualSaved;

    private int SP;
    public int maxSP = 0;

    private int AP;
    public int maxAP = 4;
    // Start is called before the first frame update
    void Start()
    {
        cachedY = savedTransform.position.y;

        maxX = savedTransform.position.x;
        minX = savedTransform.position.x - savedTransform.rect.width;

        setMaxSavedAP(0);

        setAP(maxAP);

        setSP(maxSP);

        VAP.SetActive(false);

        visualSaved.transform.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (veteran)
        {
            VAP.SetActive(true);
        }
        if (!veteran)
        {
            VAP.SetActive(false);
        }
                
    }
        
    public void addVeteran()
    {

        veteran = true;


    }

    private void setMaxSavedAP(int num)
    {
        if (num >= 0) ;
        maxSaved = num;

        if (maxSaved > 0)
        {
            visualSaved.transform.gameObject.SetActive(true);
            HandleSavedAP();
        }
        if(maxSaved == 0)
        {
            savedText.text = "SAVED AP:" + "0" + "/" + maxSaved;
            visualSaved.transform.gameObject.SetActive(false);

        }
    }

    public void EndAP()
    {
        veteran = false;
        setMaxSavedAP(AP);
        setRAP(maxSaved);
        setAP(maxAP);
        setSP(maxSP);
        
    }

    public void setAP(int thisAP)
    {

        if(thisAP <= maxAP && thisAP >= 0)
        {

            AP = thisAP;
            changeAP();
        }
        
    }

    public void setSP(int thisSP)
    {

        if (thisSP <= maxSP && thisSP >= 0)
        {
            SPnow.transform.gameObject.SetActive(true);
            SP = thisSP;
            changeSP();
        }
        if(thisSP <= 0)
        {

            SPnow.transform.gameObject.SetActive(false);

        }
        

    }

    public void setRAP(int savedAP)
    {

        if (savedAP <= maxSaved && savedAP >= 0)
        {

            currentSaved = savedAP;
            HandleSavedAP();

        }
        

    }

    private void HandleSavedAP()
    {

        savedText.text = "SAVED AP:" + currentSaved + "/" + maxSaved;

        float currentX = MapValues(currentSaved, 0, maxSaved, minX, maxX);

        savedTransform.position = new Vector3(currentX, cachedY);

        if (currentSaved > maxSaved / 2)
        {

            visualSaved.color = new Color32((byte)MapValues(currentSaved, maxSaved / 2, maxSaved, 255, 0), 255, 0, 255);

        }
        else
        {

            visualSaved.color = new Color32(255, (byte)MapValues(currentSaved, 0, maxSaved / 2, 0, 255), 0, 255);

        }

    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {

        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }

    private void changeSP()
    {

        switch (SP)
        {

            case 3:
                SP1.SetActive(true);
                SP2.SetActive(true);
                SP3.SetActive(true);
                break;
            case 2:
                SP1.SetActive(true);
                SP2.SetActive(true);
                SP3.SetActive(false);
                break;
            case 1:
                SP1.SetActive(true);
                SP2.SetActive(false);
                SP3.SetActive(false);
                break;
            case 0:
                SP1.SetActive(false);
                SP2.SetActive(false);
                SP3.SetActive(false);
                break;

        }

    }

    private void changeAP()
    {

        switch (AP)
        {

            case 5:
                AP1.SetActive(true);
                AP2.SetActive(true);
                AP3.SetActive(true);
                AP4.SetActive(true);
                AP5.SetActive(true);
                break;
            case 4:
                AP1.SetActive(true);
                AP2.SetActive(true);
                AP3.SetActive(true);
                AP4.SetActive(true);
                AP5.SetActive(false);
                break;
            case 3:
                AP1.SetActive(true);
                AP2.SetActive(true);
                AP3.SetActive(true);
                AP4.SetActive(false);
                AP5.SetActive(false);
                break;
            case 2:
                AP1.SetActive(true);
                AP2.SetActive(true);
                AP3.SetActive(false);
                AP4.SetActive(false);
                AP5.SetActive(false);
                break;
            case 1:
                AP1.SetActive(true);
                AP2.SetActive(false);
                AP3.SetActive(false);
                AP4.SetActive(false);
                AP5.SetActive(false);
                break;
            case 0:
                AP1.SetActive(false);
                AP2.SetActive(false);
                AP3.SetActive(false);
                AP4.SetActive(false);
                AP5.SetActive(false);
                break;

        }

        APnow.text = "AP: " + AP;

    }
}
