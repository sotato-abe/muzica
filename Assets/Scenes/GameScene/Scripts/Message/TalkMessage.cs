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

    public static TalkMessage GetDefaultMessage(MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Encount:
                return new TalkMessage(messageType, MessagePanelType.Default, "ちょっとつきあえや");
            case MessageType.Attack:
                return new TalkMessage(messageType, MessagePanelType.Surprise, "くらえ");
            case MessageType.Recovery:
                return new TalkMessage(messageType, MessagePanelType.Default, "これで大丈夫");
            case MessageType.Damage:
                return new TalkMessage(messageType, MessagePanelType.Surprise, "いてぇ");
            case MessageType.Miss:
                return new TalkMessage(messageType, MessagePanelType.Fear, "ミスった！");
            case MessageType.Escape:
                return new TalkMessage(messageType, MessagePanelType.Default, "まて !");
            case MessageType.Win:
                return new TalkMessage(messageType, MessagePanelType.Default, "よし");
            case MessageType.Lose:
                return new TalkMessage(messageType, MessagePanelType.Fear, "くそぅ");
            case MessageType.Talk:
                return new TalkMessage(messageType, MessagePanelType.Default, "おい、話があるんだ");
            case MessageType.Question:
                return new TalkMessage(messageType, MessagePanelType.Default, "?");
            case MessageType.Entrance:
                return new TalkMessage(messageType, MessagePanelType.Default, "いらっしゃい");
            case MessageType.Greetings:
                return new TalkMessage(messageType, MessagePanelType.Default, "こんにちは");
            case MessageType.Safe:
                return new TalkMessage(messageType, MessagePanelType.Surprise, "セーフ");
            default:
                return new TalkMessage(messageType, MessagePanelType.Default, "");
        }
    }
}
