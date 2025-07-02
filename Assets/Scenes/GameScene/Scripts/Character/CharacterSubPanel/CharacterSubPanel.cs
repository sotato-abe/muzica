using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSubPanel : SlidePanel
{
    [SerializeField] Image characterImage;
    [SerializeField] BlowingPanel blowingPanel;

    public void SetCharacter(Sprite image)
    {
        characterImage.sprite = image;
    }

    public void SetTalkMessage(TalkMessage talkMessage)
    {
        blowingPanel.gameObject.SetActive(true);
        blowingPanel.AddMessageList(talkMessage);
    }
}

