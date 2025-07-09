using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TreasureInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;

    public void SetInfo(Treasure treasure)
    {
        gameObject.SetActive(true);
        descriptionText.text = treasure.Base.Description;
    }
}