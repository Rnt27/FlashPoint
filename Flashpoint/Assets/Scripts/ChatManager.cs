using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    public string username;

    public int maxMessages = 30;

    public GameObject charPanel, textObject;

    public InputField charBox;

    public Color playerMessage, systemInfo;//receivedMessage

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {

    }

    void Update()
    {
        if (charBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + charBox.text, Message.MessageType.playerMessage);
                charBox.text = "";
            }
        }
        else
        {
            if (!charBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                charBox.ActivateInputField();
            }
        }
        if (!charBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("System: You pressed the bar!", Message.MessageType.systemInfo);
                Debug.Log("Space");
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, charPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = systemInfo;
        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;

                /*case Message.MessageType.receivedMessage:
                    color = receivedMessage;
                    break;
                */
        }

        return color;
    }
}
[System.Serializable]

public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;
    public enum MessageType
    {
        playerMessage,
        systemInfo
        //receivedMessage
    }
}
