using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class SaveManagement : MonoBehaviour
{
    public Setting setting;
    public const string RELATIVE_PATH = "setting.json";

    public TextMeshProUGUI text;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerData playerData = PlayerDataConverter();
            setting.playerData = playerData;
            Persistance.Save(RELATIVE_PATH, setting);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Load setting");
            setting = Persistance.Load<Setting>(RELATIVE_PATH);
        }
        text.enableWordWrapping = true;
        text.overflowMode = TextOverflowModes.Overflow; // はみ出す場合の挙動も選べる
        text.text = JsonConvert.SerializeObject(setting, Formatting.None);
    }
    
    private PlayerData PlayerDataConverter()
    {
        PlayerCharacter character = PlayerController.Instance.PlayerCharacter;
        PlayerData playerData = new PlayerData
        {
            characterId = 0,
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
            exp = character.Exp
        };

        // Add equipped items and bag items
        // for (int i = 0; i < EQUIPMENT_COUNT; i++)
        // {
        //     playerData.equippedItemIndexList[i, 0] = character.EquippedItems[i].Id;
        // }
        // for (int i = 0; i < character.BagItemList.Count; i++)
        // {
        //     playerData.bagItemIndexList[i, 0] = character.BagItemList[i].Id;
        // }

        return playerData;
    }
}

[System.Serializable]
public class Setting
{
    public PlayerData playerData;
    public int resolutionIndex = 0;
    public bool isFullScreen = true;
}

[System.Serializable]
public class PlayerData
{
    public int characterId = 0;
    // status
    public int maxLife = 100;
    public int currentLife = 100;
    public int maxBattery = 50;
    public int currentBattery = 50;
    public int currentSoul = 50;
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
    public int level = 1;
    public int skillPoint = 1;
    public int exp = 0;

    // Belongings
    public int rightEquipmentIndex = 20;
    public int leftEquipmentIndex = 21;
    public int[,] equippedItemIndexList = new int[5, 3];
    public int[,] pocketItemIndexList = new int[5, 2];
    public int[,] storageCommandIndexList = new int[5, 4];
    public int[,] bagItemIndexList = new int[5, 4];
    public int[,] slotCommandIndexList = new int[12, 2];
}
