using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SelectElement : MonoBehaviour
{
    [SerializeField] Image targetCursol;
    [SerializeField] TextMeshProUGUI titleText;
    private int index;

    public void SetActiveCursol(bool isActive)
    {
        targetCursol.enabled = isActive;
    }

    public void SetIndex(int idx)
    {
        index = idx;
    }

    public void SetElementText(string name)
    {
        titleText.text = name;
    }
}
