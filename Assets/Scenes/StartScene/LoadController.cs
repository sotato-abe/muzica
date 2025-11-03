using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] LoadDataButton loadDataButton1;
    [SerializeField] LoadDataButton loadDataButton2;
    [SerializeField] LoadDataButton loadDataButton3;
    [SerializeField] private PlayerCharacter sola;
    [SerializeField] private PlayerCharacter huh;

    public const string FILE_NAME1 = "playData1.json";
    public const string FILE_NAME2 = "playData2.json";
    public const string FILE_NAME3 = "playData3.json";

    private enum CharacterIndex
    {
        Sola,
        Huh
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        LoadData();
        loadDataButton1.OnStartGame += LoadGame;
        loadDataButton2.OnStartGame += LoadGame;
        loadDataButton3.OnStartGame += LoadGame;
    }

    private void LoadData()
    {
        PlayData playData1 = Persistance.Load<PlayData>(FILE_NAME1);
        PlayData playData2 = Persistance.Load<PlayData>(FILE_NAME2);
        PlayData playData3 = Persistance.Load<PlayData>(FILE_NAME3);

        if (playData1 != null)
        {
            SimpleData simpleData = new SimpleData(playData1);
            loadDataButton1.Setup(simpleData);
        }
        if (playData2 != null)
        {
            SimpleData simpleData = new SimpleData(playData2);
            loadDataButton2.Setup(simpleData);
        }
        if (playData3 != null)
        {
            SimpleData simpleData = new SimpleData(playData3);
            loadDataButton3.Setup(simpleData);
        }
    }

    void StartGame()
    {

        PlayData selectedPlayData = PlayerDataConverter(CharacterIndex.Sola);
        GameScene.selectedPlayData = selectedPlayData;
        SceneManager.LoadScene("GameScene");
    }

    void LoadGame(int index)
    {
        PlayData selectedPlayData = index switch
        {
            1 => Persistance.Load<PlayData>(FILE_NAME1),
            2 => Persistance.Load<PlayData>(FILE_NAME2),
            3 => Persistance.Load<PlayData>(FILE_NAME3),
            _ => null,
        };
        GameScene.selectedPlayData = selectedPlayData;
        SceneManager.LoadScene("GameScene");
    }

    private PlayData PlayerDataConverter(CharacterIndex characterIndex)
    {
        PlayData playData = new PlayData();
        PlayerCharacter character = characterIndex switch
        {
            CharacterIndex.Sola => sola,
            CharacterIndex.Huh => huh,
            _ => null
        };
        character.Init();

        if (character == null) return playData;

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
        playData.playerData = playerData;
        playData.position = character.Base.Birthplace.Position;
        playData.time = new DateTime(2030, 1, 1); 

        return playData;
    }
}
