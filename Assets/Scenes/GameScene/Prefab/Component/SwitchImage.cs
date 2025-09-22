using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image backimage;

    [SerializeField] Sprite firstSprite;
    [SerializeField] Sprite secondSprite;
    [SerializeField] Sprite thirdSprite;

    public void Switch(int index)
    {
        Debug.Log("SwitchImage :" +index);
        if (index == 0)
        {
            image.sprite = firstSprite;
            backimage.sprite = firstSprite;
            image.gameObject.SetActive(true);
            backimage.gameObject.SetActive(true);
        }
        else if (index == 1)
        {
            image.sprite = secondSprite;
            backimage.sprite = secondSprite;
            image.gameObject.SetActive(true);
            backimage.gameObject.SetActive(true);
        }
        else if (index == 2)
        {
            image.sprite = thirdSprite;
            backimage.sprite = thirdSprite;
            image.gameObject.SetActive(true);
            backimage.gameObject.SetActive(true);
        }
        else
        {
            image.gameObject.SetActive(false);
            backimage.gameObject.SetActive(false);
        }
    }
}