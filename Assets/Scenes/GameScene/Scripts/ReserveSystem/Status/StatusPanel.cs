using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusPanel : Panel
{
    PlayerCharacter playerCharacter;
    [SerializeField] CharacterCard characterCard;
    [SerializeField] StatusWindow statusWindow;

    private void Start()
    {
        this.playerCharacter = PlayerController.Instance.PlayerCharacter;
        characterCard.Setup(this.playerCharacter);
        statusWindow.SetCharacter(this.playerCharacter);
    }

    private void OnEnable()
    {
        if (playerCharacter == null)
        {
            playerCharacter = PlayerController.Instance?.PlayerCharacter;
            if (playerCharacter == null)
            {
                Debug.LogWarning("OnEnable: PlayerCharacter がまだ初期化されていません。");
                return;
            }
        }

        characterCard.Setup(playerCharacter);
        statusWindow.SetCharacter(playerCharacter);
    }
}
