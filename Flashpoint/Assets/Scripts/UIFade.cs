using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFade : MonoBehaviour
{

    Image image;
    Image nameGame;
    Image start;
    Image click;

    Color tempColor;
    Color tempColor2;
    Color tempColor3;
    Color tempColor4;
    float Fade = 1;
    float Fade2 = 0;
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
        
        if (tempColor2.a > 0.9)
        {

            StartCoroutine("Blink");
            
        }
        
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {

            SceneManager.LoadScene("ChooseDifficulty");

        }

    }

    //Fade in the letters
    public void FadeIn()
    {

        tempColor2.a += difference * Time.time; 
        tempColor3.a += difference * Time.time; 
        
        nameGame.color = tempColor2;
        start.color = tempColor3;
        
    }
    
    //Starting fade out the focus
    public void FadeOut()
    {

        Fade -= difference * Time.time;
        tempColor.a = Fade;
        image.color = tempColor;

    }

    //blinking text
    public IEnumerator Blink()
    {

        if (Fade2 == 0)
        {
            yield return new WaitForSeconds(1F);
            Fade2 = 1;
            tempColor4.a = Fade2;
            click.color = tempColor4;

        }

        if (Fade2 == 1)
        {

            yield return new WaitForSeconds(1F);
            Fade2 = 0;
            tempColor4.a = Fade2;
            click.color = tempColor4;

        }
        
    }
 
}
