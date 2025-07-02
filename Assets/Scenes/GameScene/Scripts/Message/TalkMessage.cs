using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TalkMessage
{
    public MessageType messageType = MessageType.Question;
    public MessagePanelType messagePanelType = MessagePanelType.Default;
    public string message = string.Empty;

    public TalkMessage(MessageType messageType, MessagePanelType messagePanelType, string message)
    {
        this.messageType = messageType;
        this.messagePanelType = messagePanelType;
        this.message = message;
    }
}
