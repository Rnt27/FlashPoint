using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Image image;
    int numberOfClicks;
    public Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        originalColor = image.color;

        numberOfClicks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfClicks <= 0)
        {
            InvokeRepeating("Blink", 0, 0.4F);
        }
        else
        {

            StopCoroutine("Blink");

        }

        if (Input.GetMouseButtonDown(0))
        {
            numberOfClicks++;
        }
    }

    public void setColor(Color c)
    {

        image.color = c;

    }

    public IEnumerator Blink()
    {
        image.color = originalColor;
        yield return new WaitForSeconds(0.2F);
        image.color = new Color(255, 255, 255, 100);
        //yield return new WaitForSeconds(0.2F);
    }
}
    
