using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillPoint : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointText;
    public void SetPoint(int point)
    {
        if (point <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        pointText.text = point.ToString();
    }
}
