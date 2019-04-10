using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameMsg : MonoBehaviour
{
    public GameObject panel;
    public int Frame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Frame <= 10)
        {
            Frame++;
        }
        
    }

    public void ShowMessage(string msg)
    {
        Debug.Log("show Message");
        
        panel.transform.GetComponentInChildren<Text>().text = msg;
        panel.SetActive(true);
        StartCoroutine(MyWait());
        
        Frame = 0;

    }

    private IEnumerator MyWait()
    {
        yield return new WaitForSeconds(1);
        panel.SetActive(false);
    }
}
