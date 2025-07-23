using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusPanel : Panel
{
    PlayerCharacter player;
    [SerializeField] CharacterCard characterCard;
    [SerializeField] StatusWindow statusWindow;

    private void Start()
    {
        this.player = PlayerController.Instance.PlayerCharacter;
        characterCard.Setup(this.player);
        statusWindow.SetCharacter(this.player);
    }

    private void OnEnable()
    {
        if (player == null)
        {
            player = PlayerController.Instance?.PlayerCharacter;
            if (player == null)
            {
                Debug.LogWarning("OnEnable: player がまだ初期化されていません。");
                return;
            }
        }

        characterCard.Setup(player);
        statusWindow.SetCharacter(player);
    }
}
