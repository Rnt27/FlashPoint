using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class POIUI : MonoBehaviour
{
    public Text victimsSaved;
    public Text victimsDead;

    public RectTransform victoryTransform;
    private float cachedY;

    private float minX;
    private float maxX;

    public int currentVictory;

    private int maxVictory = 7;
    
    public Image visualVictory;

    private int deathCount;
    private int maxDeath = 4;

    // Start is called before the first frame update
    void Start()
    {
        currentVictory = 0;
        handleVictory();
        deathCount = 0;
        handleDeath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseDeath()
    {
        if (deathCount < maxDeath)
        {
            deathCount++;

            if (deathCount == maxDeath)
            {

                SceneManager.LoadScene("Defeat");

            }

            handleDeath();
        }
    }

    private void handleDeath()
    {

        victimsDead.text = deathCount + "/" + maxDeath;
                
    }

    public void increaseVictory()
    {
        if (currentVictory < maxVictory)
        {
            currentVictory++;

            if (currentVictory == maxVictory)
            {

                SceneManager.LoadScene("Victory");

            }

            handleVictory();
        }

    }

    private void handleVictory()
    {

        victimsSaved.text = "VICTIMS SAVED     " + currentVictory + "/" + maxVictory;

        float currentX = MapValues(currentVictory, 0, maxVictory, minX, maxX);

        victoryTransform.position = new Vector3(currentX, cachedY);

        if (currentVictory > maxVictory / 2)
        {

            visualVictory.color = new Color32((byte)MapValues(currentVictory, maxVictory / 2, maxVictory, 255, 0), 255, 0, 255);

        }
        else
        {

            visualVictory.color = new Color32(255, (byte)MapValues(currentVictory, 0, maxVictory / 2, 0, 255), 0, 255);

        }

    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {

        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }
}
