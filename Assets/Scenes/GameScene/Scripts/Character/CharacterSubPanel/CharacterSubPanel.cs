using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class CharacterSubPanel : SlidePanel
{
    // public UnityAction OnEnergyOut;
    [SerializeField] Image characterImage;
    [SerializeField] Image frame;
    [SerializeField] Image nameFrame;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] public BlowingPanel blowingPanel;

    Character character;
    public Character Character => character;
    public bool fixedDisplayFlg = false; // Panelの固定表示フラグ

    public virtual void SetCharacter(Character character)
    {
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        characterNameText.text = character.Base.Name;
        this.character.OnTalkMessage += SetMessageByType;
        SetRarityColor();
        base.SetActive(true);
    }

    // TODO OwnerSubPanelに移す
    public void SetOwner(Character character)
    {
        SetCharacter(character);
        SetMessageByType(MessageType.Entrance);
    }

    private void SetRarityColor()
    {
        if (character == null) return;
        Color rarityColor = character.Base.Rarity.GetRarityColor();
        frame.color = rarityColor;
        nameFrame.color = rarityColor;
    }

    public void SetMessageByType(MessageType messageType)
    {
        UnityEngine.Debug.Log($"Setting message of type: {messageType}");
        TalkMessage talkMessage = character.GetTalkMessageByType(messageType);
        if (talkMessage != null)
        {
            StartCoroutine(SetTalkMessage(talkMessage));
        }
    }

    public IEnumerator SetTalkMessage(TalkMessage talkMessage)
    {
        if (!fixedDisplayFlg)
        {
            base.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return blowingPanel.AddMessageList(talkMessage);

        if (!fixedDisplayFlg)
        {
            yield return new WaitForSeconds(0.3f);
            base.SetActive(false);
        }
    }

    public IEnumerator JumpMotion()
    {
        float groundY = transform.position.y;
        float bounceHeight = 40f;
        float damping = 0.4f;
        float gravity = 5000f;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping; // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z); // 最後に位置を調整
    }
}

