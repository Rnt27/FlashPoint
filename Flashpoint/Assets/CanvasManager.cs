using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    
    public GameObject gameManager;
    GameObject myCanvas;
    public GameObject objectWithContextMenu;
    public GameObject actionPrefab;
    Transform actionContainer;
    public bool popupOn = false;
    // Start is called before the first frame update
    void Start()
    {
       myCanvas = this.gameObject;
       
    }

public void ShowActionMenu(string[] actions, GameObject target, string type)
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

        // Change previous selected tile color
        if (actionContainer.childCount > 0 )
        {
            //Debug.Log(objectWithContextMenu);
            GameObject previousTileWithContextMenu = objectWithContextMenu;

            UnselectObject(previousTileWithContextMenu);
        }

        // Destroy previous menu
        foreach (Transform child in actionContainer)
        {
            //GameObject a = child as GameObject;
            GameObject.Destroy(child.gameObject);

        }


        // set the current object as ObjectWithContextMenu
        SetObjectWithContextMenu(type, target);

        


        foreach (string action in actions)
        {
            string msg = null;
            //Debug.Log(action);

            GameObject go = Instantiate(actionPrefab) as GameObject;
            go.transform.SetParent(actionContainer);
            go.GetComponentInChildren<Text>().text = action;
            //Debug.Log(go.transform.GetChild(0).GetComponentInChildren<Button>());
            //Debug.Log("child: " + go.transform.GetChild(0).transform.GetChild(1));
            int ap = 1; //evaluate AP needed
            go.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = ap.ToString() + "AP";
            go.transform.localScale = new Vector3(1, 1, 1);

            Debug.Log(action);
            if (action == "Carry victim to here")
            {
                Debug.Log("msg");
                msg = "Do you want treated victim to follow?";
            }
            
            if (msg != null)
            {
                go.transform.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => showPopupWindow(popupWindow, msg));
            } 
            else
            {
                go.transform.GetChild(0).GetComponentInChildren<Button>().onClick.AddListener(() => ActivateAction(action, target, type));
            }
            
            

        }

    }

    void SetObjectWithContextMenu(string type, GameObject target)
    {
        if (type == "tile")
        {
            objectWithContextMenu = target;
            objectWithContextMenu.GetComponent<Selectable>().SetActiveContextMenu(true);
        }

        else if (type == "wall")
        {
            
            objectWithContextMenu = target;
            objectWithContextMenu.GetComponent<WallController>().SetActiveContextMenu(true);


        }

        else if (type == "door")
        {
            objectWithContextMenu = target;
            objectWithContextMenu.GetComponent<DoorController>().SetActiveContextMenu(true);


        }

    }

    void UnselectObject(GameObject anObject)
    {
        if (anObject.GetComponent<Selectable>() != null)
        {
            anObject.GetComponent<Selectable>().SetActiveContextMenu(false);
            anObject.GetComponent<Selectable>().SwitchColorToOriginal();
        }

        else if (anObject.GetComponent<WallController>() != null)
        {
            anObject.GetComponent<WallController>().SetActiveContextMenu(false);
            anObject.GetComponent<WallController>().SwitchRendererColor(Color.white);


        }

        else if (anObject.GetComponent<DoorController>() != null)
        {
            anObject.GetComponent<DoorController>().SetActiveContextMenu(false);
            anObject.GetComponent<DoorController>().SwitchRendererColor();


        }

    }

    private void ActivateAction(string action, GameObject target, string type)
    {
        if (type == "door")
        {
            target.GetComponent<Door>().ToggleDoor();
        }
        else if (action == "Move to here")
        {
            gameManager.GetComponent<Game>().setMoveButtonActive();
        }
        else if (action == "Turn Fire to Smoke")
        {
            target.GetComponent<Space>().DecrementFire();
        }
        else if (action == "Extinguish Fire")
        {
            target.GetComponent<Space>().DecrementFire();
            target.GetComponent<Space>().DecrementFire();
        }
        else if (action == "Extinguish Smoke")
        {
            target.GetComponent<Space>().DecrementFire();
        }
        else if (action == "Chop Wall")
        {
            gameManager.GetComponent<Game>().setPunchButtonActive();
        }
        else if (type == "door")
        {
            target.GetComponent<Door>().ToggleDoor();
        }
        HideActionMenu();
        UnselectObject(objectWithContextMenu);

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
