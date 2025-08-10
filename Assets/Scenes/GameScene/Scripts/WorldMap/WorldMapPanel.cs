using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapPanel : SlidePanel
{
    [SerializeField] WorldMapCamera worldMapCamera;
    [SerializeField] Title fieldTitle;

    public void SetFieldName(string name)
    {
        fieldTitle.ChangeTitle(name);
    }
}

