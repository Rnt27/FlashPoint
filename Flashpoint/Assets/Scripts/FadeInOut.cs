using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image Image;
    float Fade = 1;
    float difference = 0.1F;
    Color tempColor;

    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        tempColor = Image.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        Fade += difference * Time.time;
        if (Fade > 1) Fade = 0;
        tempColor.a = Fade;
        Image.color = tempColor;
    }
}
