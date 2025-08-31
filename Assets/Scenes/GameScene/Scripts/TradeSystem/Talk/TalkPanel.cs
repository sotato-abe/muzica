using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TalkPanel : Panel
{
    [SerializeField] ShopQuestWindow shopQuestWindow;

    public void SetPoint(Point point)
    {
        shopQuestWindow.SetPoint(point);
    }
}
