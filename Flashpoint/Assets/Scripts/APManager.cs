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

    public Image visualSaved;

    private int SP;
    public int maxSP;

    private int AP;
    public int maxAP = 4;
    // Start is called before the first frame update
    void Start()
    {
        cachedY = savedTransform.position.y;

        maxX = savedTransform.position.x;
        minX = savedTransform.position.x - savedTransform.rect.width;

        currentSaved = maxSaved;

        AP = maxAP;

        SP = maxSP;

        VAP.SetActive(false);

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

    public void addVeteran()
    {

        veteran = true;


    }

    private void setSavedAP(int num)
    {

        maxSaved = num;
        currentSaved = maxSaved;
        HandleSavedAP();

    }

    public void EndAP()
    {
        veteran = false;
        setSavedAP(AP);
        AP = maxAP;
        SP = maxSP;

    }

    private void HandleSavedAP()
    {

        savedText.text = currentSaved + "/" + maxSaved;

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
}
