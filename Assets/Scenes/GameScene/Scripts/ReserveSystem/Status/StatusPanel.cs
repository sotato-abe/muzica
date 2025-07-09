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
        playerCharacter = PlayerController.Instance.PlayerCharacter;
        characterCard.Setup(playerCharacter);
        statusWindow.SetCharacter(playerCharacter);
    }
}
