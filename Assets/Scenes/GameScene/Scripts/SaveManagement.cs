using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SaveManagement : MonoBehaviour
{
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] SlidePanel saveButtonList;
    [SerializeField] SavePanel savePanel;
    public PlayData playData;
    public const string RELATIVE_PATH = "playData.json";
    public const string FILE_NAME = "playData";
    public const string FILE_EXTENSION = ".json";

    public void SavePlayData(int index)
    {
        playData.position = WorldMapController.Instance.playerPosition;
        playData.time = ageTimePanel.ageTime;
        playData.yearsElapsed = ageTimePanel.yearsElapsed;
        PlayerData playerData = PlayerDataConverter(PlayerController.Instance.PlayerCharacter);
        playData.playerData = playerData;
        string filePath = $"{FILE_NAME}{index}{FILE_EXTENSION}";
        Persistance.Save(filePath, playData);
    }

    private void LoadPlayData(int index)
    {
        string filePath = $"{FILE_NAME}{index}{FILE_EXTENSION}";
        playData = Persistance.Load<PlayData>(filePath);
        PlayerCharacter loadCharacter = LoadPlayerCharacter(playData.playerData);
        PlayerController.Instance.SetPlayerCharacter(loadCharacter);
        ageTimePanel.TimeSlip(playData.time);
        ageTimePanel.yearsElapsed = playData.yearsElapsed;
    }

    public SimpleData GetSaveData(int index)
    {
        string filePath = $"{FILE_NAME}{index}{FILE_EXTENSION}";
        playData = Persistance.Load<PlayData>(filePath);
        if (playData == null) return null;
        return new SimpleData(playData);
    }

    public PlayerData PlayerDataConverter(PlayerCharacter character)
    {
        int characterId = CharacterDatabase.Instance.GetCharacterId(character.Base);
        PlayerData playerData = new PlayerData
        {
            characterId = characterId,
            maxLife = character.MaxLife,
            currentLife = character.Life,
            maxBattery = character.MaxBattery,
            currentBattery = character.Battery,
            currentSoul = character.Soul,
            power = character.Power,
            technique = character.Technique,
            defense = character.Defense,
            speed = character.Speed,
            luck = character.Luck,
            memory = character.Memory,
            storage = character.Storage,
            pocket = character.Pocket,
            coin = character.Coin,
            disc = character.Disc,
            key = character.Key,
            level = character.Level,
            skillPoint = character.SkillPoint,
            exp = character.Exp,
        };
        if (character.RightHandEquipment != null)
            playerData.rightEquipmentIndex = ItemDatabase.Instance.GetItemId(character.RightHandEquipment.Base);
        if (character.LeftHandEquipment != null)
            playerData.leftEquipmentIndex = ItemDatabase.Instance.GetItemId(character.LeftHandEquipment.Base);

        foreach (var item in character.BagItemList)
        {
            int itemId = ItemDatabase.Instance.GetItemId(item.Base);
            if (itemId != -1)
                playerData.bagItemIndexList.Add(itemId);
        }
        foreach (var item in character.PocketList)
        {
            int itemId = ItemDatabase.Instance.GetItemId(item.Base);
            if (itemId != -1)
                playerData.pocketItemIndexList.Add(itemId);
        }
        foreach (var command in character.StorageList)
        {
            int commandId = CommandDatabase.Instance.GetCommandId(command.Base);
            if (commandId != -1)
                playerData.storageCommandIndexList.Add(commandId);
        }
        foreach (var command in character.SlotList)
        {
            if (command == null)
            {
                playerData.slotCommandIndexList.Add(-1);
                continue;
            }
            int commandId = CommandDatabase.Instance.GetCommandId(command.Base);
            if (commandId != -1)
                playerData.slotCommandIndexList.Add(commandId);
            else
                playerData.slotCommandIndexList.Add(-1);
        }

        return playerData;
    }

    // PlayerDataからPlayerCharacterを生成
    // PlayerDataにセットされたステータスをPlayerCharacterに適用
    public PlayerCharacter LoadPlayerCharacter(PlayerData playerData)
    {
        PlayerCharacter loadPlayerCharacter = CharacterDatabase.Instance.GetPlayerCharacterFromId(playerData.characterId);
        loadPlayerCharacter.MaxLife = playerData.maxLife;
        loadPlayerCharacter.Life = playerData.currentLife;
        loadPlayerCharacter.MaxBattery = playerData.maxBattery;
        loadPlayerCharacter.Battery = playerData.currentBattery;
        loadPlayerCharacter.Soul = playerData.currentSoul;
        loadPlayerCharacter.Power = playerData.power;
        loadPlayerCharacter.Technique = playerData.technique;
        loadPlayerCharacter.Defense = playerData.defense;
        loadPlayerCharacter.Speed = playerData.speed;
        loadPlayerCharacter.Luck = playerData.luck;
        loadPlayerCharacter.Memory = playerData.memory;
        loadPlayerCharacter.Storage = playerData.storage;
        loadPlayerCharacter.Pocket = playerData.pocket;
        loadPlayerCharacter.Coin = playerData.coin;
        loadPlayerCharacter.Disc = playerData.disc;
        loadPlayerCharacter.Key = playerData.key;
        loadPlayerCharacter.Level = playerData.level;
        loadPlayerCharacter.SkillPoint = playerData.skillPoint;
        loadPlayerCharacter.Exp = playerData.exp;
        loadPlayerCharacter.ColStatus();

        loadPlayerCharacter.RightHandEquipment = ItemDatabase.Instance.GetItemFromId(playerData.rightEquipmentIndex) as Equipment;
        loadPlayerCharacter.LeftHandEquipment = ItemDatabase.Instance.GetItemFromId(playerData.leftEquipmentIndex) as Equipment;

        loadPlayerCharacter.BagItemList.Clear();
        foreach (int itemId in playerData.bagItemIndexList)
        {
            Item newItem = ItemDatabase.Instance.GetItemFromId(itemId);
            if (newItem != null)
            {
                loadPlayerCharacter.AddItemToBag(newItem);
            }
        }
        loadPlayerCharacter.PocketList.Clear();
        foreach (int itemId in playerData.pocketItemIndexList)
        {
            Item newItem = ItemDatabase.Instance.GetItemFromId(itemId);
            if (newItem != null)
            {
                loadPlayerCharacter.AddItemToPocket(newItem as Consumable);
            }
        }
        loadPlayerCharacter.StorageList.Clear();
        foreach (int commandId in playerData.storageCommandIndexList)
        {
            Command newCommand = CommandDatabase.Instance.GetCommandFromId(commandId);
            if (newCommand != null)
            {
                loadPlayerCharacter.AddCommandToStorage(newCommand);
            }
        }
        loadPlayerCharacter.SlotList.Clear();
        int slotIndex = 0;
        foreach (int commandId in playerData.slotCommandIndexList)
        {
            Command newCommand = CommandDatabase.Instance.GetCommandFromId(commandId);
            if (newCommand != null)
            {
                loadPlayerCharacter.SlotList.Add(newCommand);
            }
            else
            {
                loadPlayerCharacter.SlotList.Add(null);
            }
            slotIndex++;
        }

        return loadPlayerCharacter;
    }
}
