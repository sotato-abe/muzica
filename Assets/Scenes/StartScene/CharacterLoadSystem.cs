using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterLoadSystem : MonoBehaviour
{
    [SerializeField] StartButton startButton;
    [SerializeField] LoadDataButton loadDataButton1;
    [SerializeField] LoadDataButton loadDataButton2;
    [SerializeField] LoadDataButton loadDataButton3;
    [SerializeField] CharacterSelectController characterSelectController;
    [SerializeField] TextMeshProUGUI ageText;
    [SerializeField] TextMeshProUGUI ageBackText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI memoText;

    public const string FILE_NAME1 = "playData1.json";
    public const string FILE_NAME2 = "playData2.json";
    public const string FILE_NAME3 = "playData3.json";

    private CharacterIndex currentCharacterIndex = CharacterIndex.Sola;

    private void Start()
    {
        LoadData();
        startButton.OnClick += StartGame;
        loadDataButton1.OnStartGame += LoadGame;
        loadDataButton2.OnStartGame += LoadGame;
        loadDataButton3.OnStartGame += LoadGame;
        characterSelectController.CharacterSelect(0);
        characterSelectController.OnStartGame += StartGame;
        characterSelectController.OnCharacterSelect += SelectPlayerCharacter;
    }

    private void LoadData()
    {
        string characterPrefix = currentCharacterIndex.GetCharacterFileName();
        PlayData playData1 = Persistance.Load<PlayData>(characterPrefix + FILE_NAME1);
        PlayData playData2 = Persistance.Load<PlayData>(characterPrefix + FILE_NAME2);
        PlayData playData3 = Persistance.Load<PlayData>(characterPrefix + FILE_NAME3);

        if (playData1 != null)
        {
            SimpleData simpleData = new SimpleData(playData1);
            loadDataButton1.Setup(simpleData);
        }
        else
        {
            loadDataButton1.Setup(null);
        }
        if (playData2 != null)
        {
            SimpleData simpleData = new SimpleData(playData2);
            loadDataButton2.Setup(simpleData);
        }
        else
        {
            loadDataButton2.Setup(null);
        }

        if (playData3 != null)
        {
            SimpleData simpleData = new SimpleData(playData3);
            loadDataButton3.Setup(simpleData);
        }
        else
        {
            loadDataButton3.Setup(null);
        }
    }

    void StartGame()
    {
        PlayData selectedPlayData = PlayerDataConverter(currentCharacterIndex);
        GameScene.selectedPlayData = selectedPlayData;
        GameScene.currentCharacterIndex = currentCharacterIndex;
        ChangeSceneEffect();
    }

    public void SelectPlayerCharacter(int characterIndex)
    {
        currentCharacterIndex = (CharacterIndex)characterIndex;
        LoadData();
        SetStartButton();
    }

    private void SetStartButton()
    {
        PlayCharacterBase characterBase = CharacterDatabase.Instance.GetCharacterByIndex(currentCharacterIndex);
        if (characterBase != null)
        {
            startButton.Setup(characterBase.StoryName);
            ageText.text = characterBase.StartYear.ToString() + "~";
            ageBackText.text = characterBase.StartYear.ToString() + "~";
            timeText.text = characterBase.StartYear.ToString() + "/" + characterBase.StartMonth.ToString() + "/" + characterBase.StartDay.ToString();
            memoText.text = characterBase.Memo;
        }
    }

    void LoadGame(int index)
    {
        string characterPrefix = currentCharacterIndex.GetCharacterFileName();
        PlayData selectedPlayData = index switch
        {
            1 => Persistance.Load<PlayData>(characterPrefix + FILE_NAME1),
            2 => Persistance.Load<PlayData>(characterPrefix + FILE_NAME2),
            3 => Persistance.Load<PlayData>(characterPrefix + FILE_NAME3),
            _ => null,
        };
        GameScene.selectedPlayData = selectedPlayData;
        GameScene.currentCharacterIndex = currentCharacterIndex;
        ChangeSceneEffect();
    }

    private void ChangeSceneEffect()
    {
        // TODO：シーン移動のエフェクトを再生してからシーン移動
        // 画面が拡大しながら黒にフェードしていく感じ
        // 逆に読み込先のシーンでは黒から画面が拡大しながらフェードインしていく感じ
        SceneManager.LoadScene("GameScene");
    }

    private PlayData PlayerDataConverter(CharacterIndex characterIndex)
    {
        PlayData playData = new PlayData();
        PlayCharacterBase characterBase = CharacterDatabase.Instance.GetCharacterByIndex(characterIndex);
        PlayerCharacter character = new PlayerCharacter(characterBase.Base);
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
        playData.time = new DateTime(characterBase.StartYear, characterBase.StartMonth, characterBase.StartDay);

        return playData;
    }
}
