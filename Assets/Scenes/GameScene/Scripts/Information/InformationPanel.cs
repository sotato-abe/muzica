using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InformationPanel : SlidePanel
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Image image;

    public void SetInformation(Information information)
    {
        titleText.text = information.Base.Title;
        descriptionText.text = information.Base.Description;
        image.sprite = information.Base.Sprite;
        StartCoroutine(DisplayInformation());
    }

    private IEnumerator DisplayInformation()
    {
        this.SetActive(true);
        yield return new WaitForSeconds(5f);
        this.SetActive(false);
    }
}
