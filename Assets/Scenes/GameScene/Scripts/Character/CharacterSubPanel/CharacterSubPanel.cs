using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSubPanel : SlidePanel
{
    [SerializeField] Image characterImage;
    [SerializeField] BlowingPanel blowingPanel;
    private bool isStayRequested = false;

    public void SetCharacter(Sprite image)
    {
        characterImage.sprite = image;
    }

    public IEnumerator SetTalkMessage(TalkMessage talkMessage, bool isStay = false)
    {
        if (isStay) isStayRequested = true; // stayがリクエストされたらマーク

        bool currentlyActive = isActive;
        if (!currentlyActive)
        {
            SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        blowingPanel.gameObject.SetActive(true);
        yield return blowingPanel.AddMessageList(talkMessage);

        // ★ 最新のstayリクエストを見てから判断する
        if (!currentlyActive && !isStayRequested)
        {
            yield return new WaitForSeconds(0.3f);
            SetActive(false);
        }

        // 最後のリクエストだった場合にのみ解除（慎重にやるなら別管理してもいい）
        if (!isStay)
        {
            isStayRequested = false;
        }
    }
}

