using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseLife : MonoBehaviour
{
   public RectTransform healthTransform;
    private float cachedY;

    private float minX;
    private float maxX;

    private int currentHealth;

    private int maxHealth = 25;
    public Text healthText;

    public Image visualHealth;

    // Start is called before the first frame update
    void Start()
    {
        cachedY = healthTransform.position.y;

        maxX = healthTransform.position.x;
        minX = healthTransform.position.x - healthTransform.rect.width;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("q"))
        {

            diminishHealth();

        }
    }

    private void HandleHealth()
    {

        healthText.text = currentHealth + "/25";

        float currentX = MapValues(currentHealth, 0, maxHealth, minX, maxX);

        healthTransform.position = new Vector3(currentX, cachedY);

        if(currentHealth > maxHealth / 2)
        {

            visualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);

        }
        else
        {

            visualHealth.color = new Color32(255,(byte)MapValues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);

        }

    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {

        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }

    public void diminishHealth()
    {

        currentHealth--;

        HandleHealth();
    }
}
