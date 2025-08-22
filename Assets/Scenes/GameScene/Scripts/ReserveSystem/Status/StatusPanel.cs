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
        PlayerController.Instance.OnPlayerCharacterSet += HandlePlayerSet;

        // すでにセット済みなら即反映
        if (PlayerController.Instance.PlayerCharacter != null)
        {
            HandlePlayerSet(PlayerController.Instance.PlayerCharacter);
        }
    }

    private void OnDisable()
    {
        PlayerController.Instance.OnPlayerCharacterSet -= HandlePlayerSet;
    }

    private void HandlePlayerSet(PlayerCharacter newplayer)
    {
        this.player = newplayer;
        characterCard.Setup(player);
        statusWindow.SetCharacter(player);
    }
}
