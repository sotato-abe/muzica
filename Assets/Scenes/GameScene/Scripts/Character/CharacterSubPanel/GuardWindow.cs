using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuardWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI counterBackText;

    public void SetGuardCounter(int counter)
    {
        counterText.text = counter.ToString();
        counterBackText.text = counter.ToString();
    }
}