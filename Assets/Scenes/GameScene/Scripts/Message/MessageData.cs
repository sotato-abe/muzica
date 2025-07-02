using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMessageData", menuName = "Message/Message Data")]
public class MessageData : ScriptableObject
{
    public MessageIconType type;
    [SerializeField] public Sprite icon;
}
