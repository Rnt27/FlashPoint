using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{

    Image image;
    Image nameGame;
    Image start;
    Image click;

    public Color tempColor;
    public Color tempColor2;
    Color tempColor3;
    Color tempColor4;
    float Fade = 1;
    float difference = 0.01F;

    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("FadeOut").GetComponent<Image>();
        nameGame = GameObject.Find("NameOfGame").GetComponent<Image>();
        start = GameObject.Find("GameStart").GetComponent<Image>();
        click = GameObject.Find("ClickAnywhere").GetComponent<Image>();

        tempColor = image.color;
        tempColor2 = nameGame.color;
        tempColor3 = start.color;
        tempColor4 = click.color;

        tempColor2.a = 0;
        tempColor3.a = 0;
        tempColor4.a = 0;

        nameGame.color = tempColor2;
        start.color = tempColor3;
        click.color = tempColor4;

    }

    // Update is called once per frame
    void Update()
    {
        if (tempColor.a != 0)
        {
            FadeOut();
            
        }
        
        if(tempColor.a < 0.1){

            if (tempColor2.a != 1) {


                FadeIn();

            }
                        
        }

        if (tempColor4.a > 0 && tempColor2.a > 0.9)
        {

            tempColor4.a -= 0.1F * Time.time; ;
            click.color = tempColor4;

        }
        if (tempColor4.a < 1 && tempColor2.a > 0.9)
        {

            tempColor4.a += 0.1F * Time.time; ;
            click.color = tempColor4;

        }
    }

    public void FadeIn()
    {

        //Fade += difference * Time.time;

        tempColor2.a += difference * Time.time; 
        tempColor3.a += difference * Time.time; 
        //tempColor4.a += difference * Time.time;

        nameGame.color = tempColor2;
        start.color = tempColor3;
        //click.color = tempColor4;

        //StartCoroutine(Fading(uiObject, uiObject.alpha, 1));

    }
    
    //Starting fade out routine
    public void FadeOut()
    {

        Fade -= difference * Time.time;
        tempColor.a = Fade;
        image.color = tempColor;

        //StartCoroutine(Fading(image, tempColor.a, 0));

    }

   /* //for fading
    public IEnumerator Fading(Image image, float begin, float end)
    {
        float timeStartLerping = Time.time;
        float timeSinceLerping = Time.time - timeStartLerping;

        //Where we are in the fade
        float percentageCompleted = timeSinceLerping / 0.5F;

        while(true)
        {
            timeSinceLerping = Time.time - timeStartLerping;
            percentageCompleted = timeSinceLerping / 0.5F;

            float current = Mathf.Lerp(begin, end, percentageCompleted);

            tempColor.a = current;

            image.color = tempColor;

            if (percentageCompleted >= 1) break;

            //run at speed of update function
            yield return new WaitForEndOfFrame();

        }

        nameGame.SetActive(true);
        start.SetActive(true);
        click.SetActive(true);

    }
    */
}
