using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;

public class SaveManagement : MonoBehaviour
{
    [SerializeField] AgeTimePanel ageTimePanel;
    public SaveData setting;
    public const string RELATIVE_PATH = "setting.json";

    public TextMeshProUGUI text;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            setting.position = WorldMapController.Instance.playerPosition;
            setting.time = ageTimePanel.ageTime;
            PlayerData playerData = PlayerDataConverter();
            setting.playerData = playerData;
            Persistance.Save(RELATIVE_PATH, setting);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Load setting");
            setting = Persistance.Load<SaveData>(RELATIVE_PATH);
        }
        text.enableWordWrapping = true;
        text.overflowMode = TextOverflowModes.Overflow; // はみ出す場合の挙動も選べる
        text.text = JsonConvert.SerializeObject(setting, Formatting.None);
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
        if(character.RightHandEquipment != null)
            playerData.rightEquipmentIndex = ItemDatabase.Instance.GetItemId(character.RightHandEquipment.Base);
        if(character.LeftHandEquipment != null)
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
            int commandId = CommandDatabase.Instance.GetCommandId(command.Base);
            if (commandId != -1)
                playerData.slotCommandIndexList.Add(commandId);
        }

        return playerData;
    }
}

[System.Serializable]
public class SaveData
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
