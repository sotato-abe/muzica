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

    public IEnumerator SetTalkMessage(TalkMessage talkMessage)
    {
        bool currentlyActive = isActive;
        if (!currentlyActive)
        {
            SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        blowingPanel.gameObject.SetActive(true);
        yield return blowingPanel.AddMessageList(talkMessage);
        if (!currentlyActive)
        {
            yield return new WaitForSeconds(0.3f);
            SetActive(false);
        }
    }
}

