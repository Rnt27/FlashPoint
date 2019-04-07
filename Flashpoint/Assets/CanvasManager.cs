using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameObject objectWithContextMenu;
    GameObject myCanvas;
    public GameObject actionPrefab;
    Transform actionContainer;
    public bool popupOn = false;
    // Start is called before the first frame update
    void Start()
    {
       myCanvas = this.gameObject;
       
    }

public void ShowActionMenu(string[] actions, GameObject anObject, string type)
    {
        
        //find the context menu

        GameObject contextMenu = myCanvas.transform.Find("ContextMenu").gameObject;
        GameObject popupWindow = myCanvas.transform.Find("popupWindow").gameObject;
        //Debug.Log("context= " + contextMenu);
        //Debug.Log("popup= " + popupWindow);

        contextMenu.SetActive(true);

        Transform actionContainer = contextMenu.transform.Find("actionMenu");
        Vector2 menuPosition = Input.mousePosition;
        contextMenu.transform.position = new Vector3(menuPosition.x + 70, menuPosition.y - 30, 0);

        //Debug.Log("actionContainer= " + actionContainer);
        if (actionContainer.childCount > 0)
        {
            //Debug.Log(objectWithContextMenu);
            GameObject previousTileWithContextMenu = objectWithContextMenu;

            if (previousTileWithContextMenu.GetComponent<Selectable>() != null)
            {
                previousTileWithContextMenu.GetComponent<Selectable>().SetActiveContextMenu(false);
                previousTileWithContextMenu.GetComponent<Selectable>().SwitchColorToOriginal();
            }

            else if (previousTileWithContextMenu.GetComponent<WallController>() != null)
            {
                previousTileWithContextMenu.GetComponent<WallController>().SetActiveContextMenu(false);
                previousTileWithContextMenu.GetComponent<WallController>().SwitchRendererColor(Color.white);


            }

            else if (previousTileWithContextMenu.GetComponent<DoorController>() != null)
            {
                previousTileWithContextMenu.GetComponent<DoorController>().SetActiveContextMenu(false);
                previousTileWithContextMenu.GetComponent<DoorController>().SwitchRendererColor();


            }



        }
        // Destroy previous menu
        foreach (Transform child in actionContainer)
        {
            //GameObject a = child as GameObject;
            GameObject.Destroy(child.gameObject);

        }
        string msg = "hello";
        // set the current object with context menu
        if (type == "tile")
        {
            objectWithContextMenu = anObject;
            objectWithContextMenu.GetComponent<Selectable>().SetActiveContextMenu(true);
        }

        else if (type == "wall")
        {
            msg = null;
            objectWithContextMenu = anObject;
            objectWithContextMenu.GetComponent<WallController>().SetActiveContextMenu(true);


        }

        else if (type == "door")
        {
            msg = null;
            objectWithContextMenu = anObject;
            objectWithContextMenu.GetComponent<DoorController>().SetActiveContextMenu(true);


        }



        foreach (string action in actions)
        {
            //Debug.Log(action);

            GameObject go = Instantiate(actionPrefab) as GameObject;
            go.transform.SetParent(actionContainer);
            go.GetComponentInChildren<Text>().text = action;
            //Debug.Log(go.transform.GetChild(0).GetComponentInChildren<Button>());
            //Debug.Log("child: " + go.transform.GetChild(0).transform.GetChild(1));
            int ap = 1; //evaluate AP needed
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ap.ToString() + "AP";
            go.transform.localScale = new Vector3(1, 1, 1);
            
            if (msg != null)
            {
                go.transform.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => showPopupWindow(popupWindow, msg));
            } 
            
            

        }

    }
    void HideActionMenu()
    {
        //no_menu = true;

        //find the context menu
        GameObject contextMenu = myCanvas.transform.Find("ContextMenu").gameObject;

        contextMenu.SetActive(false);



    }

    void showPopupWindow(GameObject popupWindow, string msg)
    {
        popupOn = true;
        //tileWithContextMenu.GetComponent<Selectable>().SetPopupOn(true);
        HideActionMenu();
        //Debug.Log("test");
        Transform question = popupWindow.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1);

        //Get question text and change it to msg
        question.transform.GetChild(1).GetComponent<Text>().text = msg;
        //Debug.Log("child: " + popupWindow.transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform.GetChild(1));
        popupWindow.GetComponent<Animator>().Play("Exit Panel In");

        //Debug.Log("no button = "+ question.transform.GetChild(2).transform.GetChild(0));
        Button no = question.transform.GetChild(2).transform.GetChild(0).GetComponentInChildren<Button>();
        no.onClick.AddListener(() => noPopup());

    }

    void noPopup()
    {
        popupOn = false;
        objectWithContextMenu.GetComponent<Selectable>().SetActiveContextMenu(false);
        Debug.Log("no button clicked");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
