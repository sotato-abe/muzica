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
    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    public PlayData playData;
    public const string RELATIVE_PATH = "playData.json";

    public TextMeshProUGUI text;

    private void Awake()
    {
        saveButton.onClick.AddListener(SavePlayData);
        loadButton.onClick.AddListener(LoadPlayData);
    }

    private void SavePlayData()
    {
        playData.position = WorldMapController.Instance.playerPosition;
        playData.time = ageTimePanel.ageTime;
        PlayerData playerData = PlayerDataConverter();
        playData.playerData = playerData;
        Persistance.Save(RELATIVE_PATH, playData);
    }

    private void LoadPlayData()
    {
        playData = Persistance.Load<PlayData>(RELATIVE_PATH);
        PlayerCharacter loadCharacter = LoadPlayerCharacter(playData.playerData);
        PlayerController.Instance.SetPlayerCharacter(loadCharacter);
        WorldMapController.Instance.WarpPlayerCoordinate(playData.position);
        ShowDebugLog();
    }

    private void ShowDebugLog()
    {
        text.enableWordWrapping = true;
        text.overflowMode = TextOverflowModes.Overflow; // はみ出す場合の挙動も選べる
        text.text = JsonConvert.SerializeObject(playData, Formatting.None);
    }

    private PlayerData PlayerDataConverter()
    {
        PlayerCharacter character = PlayerController.Instance.PlayerCharacter;
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
                Debug.LogWarning("SlotCommand is null, skipping.");
                continue;
            }
            int commandId = CommandDatabase.Instance.GetCommandId(command.Base);
            if (commandId != -1)
                playerData.slotCommandIndexList.Add(commandId);
        }

        return playerData;
    }

    // PlayerDataからPlayerCharacterを生成
    // PlayerDataにセットされたステータスをPlayerCharacterに適用
    private PlayerCharacter LoadPlayerCharacter(PlayerData playerData)
    {
        PlayerCharacter loadPlayerCharacter = CharacterDatabase.Instance.GetPlayerCharacterFromId(playerData.characterId);
        Debug.Log("LoadPlayerCharacter: " + loadPlayerCharacter.Base.name);
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

        loadPlayerCharacter.RightHandEquipment = ItemDatabase.Instance.GetItemFromId(playerData.rightEquipmentIndex) as Equipment;
        loadPlayerCharacter.LeftHandEquipment = ItemDatabase.Instance.GetItemFromId(playerData.leftEquipmentIndex) as Equipment;

        loadPlayerCharacter.BagItemList.Clear();
        foreach(int itemId in playerData.bagItemIndexList)
        {
            Item newItem = ItemDatabase.Instance.GetItemFromId(itemId);
            if (newItem != null)
            {
                loadPlayerCharacter.AddItemToBag(newItem);
            }
            else
            {
                Debug.LogWarning("未対応のItemBase型：" + itemId);
            }
        }
        loadPlayerCharacter.PocketList.Clear();
        foreach(int itemId in playerData.pocketItemIndexList)
        {
            Item newItem = ItemDatabase.Instance.GetItemFromId(itemId);
            if (newItem != null)
            {
                loadPlayerCharacter.AddItemToPocket(newItem as Consumable);
            }
            else
            {
                Debug.LogWarning("未対応のItemBase型：" + itemId);
            }
        }
        loadPlayerCharacter.StorageList.Clear();
        loadPlayerCharacter.SlotList.Clear();




        return loadPlayerCharacter;
    }
}

[System.Serializable]
public class PlayData
{
    public DateTime time = DateTime.Now;
    public Vector2Int position;
    public PlayerData playerData;
}

public class PlayerData
{
    public int characterId = 0;
    // status
    public int maxLife = 0;
    public int currentLife = 0;
    public int maxBattery = 0;
    public int currentBattery = 0;
    public int currentSoul = 0;
    public int power = 0;
    public int technique = 0;
    public int defense = 0;
    public int speed = 0;
    public int luck = 0;
    public int memory = 0;
    public int storage = 0;
    public int pocket = 0;

    // Currency
    public int coin = 0;
    public int disc = 0;
    public int key = 0;
    public int level = 0;
    public int skillPoint = 0;
    public int exp = 0;

    // Belongings
    public int rightEquipmentIndex = -1;
    public int leftEquipmentIndex = -1;
    public List<int> bagItemIndexList = new List<int>();
    public List<int> pocketItemIndexList = new List<int>();
    public List<int> storageCommandIndexList = new List<int>();
    public List<int> slotCommandIndexList = new List<int>();
}
