using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : SlidePanel
{
    [SerializeField] WorldMapCamera worldMapCamera;
    [SerializeField] Title fieldName;

    public void SetFieldName(string name)
    {
        StartCoroutine(fieldName.TypeTitle(name));
    }
}

