using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusPanel : Panel, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop");
    }
}
