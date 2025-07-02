using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Message
{
    public Sprite sprite;
    public string messageText;

     public Message(Sprite sprite, string messageText)
    {
        this.sprite = sprite;
        this.messageText = messageText;
    }
}
