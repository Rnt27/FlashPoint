using BayatGames.SaveGameFree.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSavedGame : MonoBehaviour
{
    public Button loadFamily;
    public Button loadFamily2;
    public GameObject saveobject;
    // Start is called before the first frame update
    void Start()
    {
        saveobject  = GameObject.Find("Save");
        loadFamily = this.transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Button>();
        loadFamily2 = this.transform.GetChild(5).transform.GetChild(2).transform.GetChild(1).GetComponent<Button>();
        loadFamily.onClick.AddListener(() => saveobject.GetComponent<SaveMyGame>().SetLoadOnStartToTrue(1));
        loadFamily2.onClick.AddListener(() => saveobject.GetComponent<SaveMyGame>().SetLoadOnStartToTrue(2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
