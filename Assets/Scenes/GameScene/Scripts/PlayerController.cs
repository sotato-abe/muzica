using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態とアクションを管理するシングルトンクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }
    #endregion

    #region Serialized Fields
    [Header("Player Components")]
    [SerializeField] private PlayerSubPanel playerSubPanel;
    [SerializeField] private CurrencyPanel currencyPanel;
    [SerializeField] private PlayerCharacter defaultPlayer; // 初期用だけ
    [SerializeField] private SaveManagement saveManagement;
    [SerializeField] private FieldPlayer fieldPlayer;
    #endregion

    public PlayerCharacter PlayerCharacter { get; private set; }
    public event System.Action<PlayerCharacter> OnPlayerCharacterSet;
    public EventType CurrentEventType { get; private set; } = EventType.Default;
    [SerializeField] private MessagePanel messagePanel;

    #region Unity Lifecycle
    void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        // 初期状態はインスペクタでセットされたデフォルトを使う
        PlayData selectedPlayData = GameScene.selectedPlayData;
        if (selectedPlayData != null)
        {
            PlayerCharacter = saveManagement.LoadPlayerCharacter(GameScene.selectedPlayData.playerData);
        }
        if (PlayerCharacter == null && defaultPlayer != null)
        {
            defaultPlayer.Init();
            SetPlayerCharacter(defaultPlayer);
        }
        playerSubPanel.SetCharacter(PlayerCharacter);
        PlayerCharacter.onLevelUp += LevelUp;
        UpdateCurrencyPanel();
    }
    #endregion

    #region Initialization
    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public void SetPlayerCharacter(PlayerCharacter loadCharcter)
    {
        if (loadCharcter == null)
        {
            Debug.LogError("ロードするキャラクターがnullです。");
            return;
        }
        PlayerCharacter = loadCharcter;
        PlayerCharacter.Init();
        playerSubPanel.SetCharacter(PlayerCharacter);
        UpdateCurrencyPanel();

        OnPlayerCharacterSet?.Invoke(PlayerCharacter);
    }

    public void ChangeEventType(EventType eventType)
    {
        CurrentEventType = eventType;
    }

    #region Player Message
    public void SetPlayerMessage(TalkMessage talkMessage)
    {
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage));
    }
    public void SetPlayerMessageByType(MessageType messageType)
    {
        playerSubPanel.SetMessageByType(messageType);
    }
    #endregion

    #region Battle System
    /// <summary>
    /// バトル報酬を追加
    /// </summary>
    public void AddBattleReward(int exp, int coin, List<Item> items)
    {
        PlayerCharacter.AddExp(exp);
        PlayerCharacter.AddCoin(coin);

        foreach (Item item in items)
        {
            if (item != null)
            {
                AddItemToBag(item);
            }
        }

        UpdateCurrencyPanel();
    }

    public void LevelUp()
    {
        playerSubPanel.SetStatusText("Level Up!!");
    }

    public void SetStatusText(string status)
    {
        playerSubPanel.SetStatusText(status);
    }
    #endregion

    #region Item Management

    public bool HasBagSpace(int additionalItems = 1)
    {
        return PlayerCharacter.Bag >= PlayerCharacter.BagItemList.Count + additionalItems;
    }

    /// <summary>
    /// アイテムをバッグに追加
    /// </summary>
    public void AddItemToBag(Item item)
    {
        if (item == null) return;

        if (PlayerCharacter.Bag <= PlayerCharacter.BagItemList.Count)
        {
            Debug.LogWarning($"バックの容量を超えています。アイテムを追加できません。");
            messagePanel.AddMessage(MessageIconType.Bag, $"バックがパンパンだった。{item.Base.Name}を落としてしまった。");
            FieldController.Instance.DropPlayerItem(item.Clone());
            return;
        }
        PlayerCharacter.AddItemToBag(item.Clone());
    }

    /// <summary>
    /// アイテムをポケットに追加
    /// </summary>
    public void AddItemToPocket(Item item)
    {
        if (PlayerCharacter.ColPocket <= PlayerCharacter.PocketList.Count)
        {
            Debug.LogWarning("ポケットの容量を超えています。アイテムを追加できません。");
            FieldController.Instance.DropPlayerItem(item.Clone());
            return;
        }

        if (item is Consumable consumable)
        {
            PlayerCharacter.AddItemToPocket(consumable.Clone() as Consumable);
        }
    }

    public Equipment GetEquipmentByBodyPart(BodyPartType bodyPartType)
    {
        if (bodyPartType == BodyPartType.None) return null;
        else if (bodyPartType == BodyPartType.RightHand)
            return PlayerCharacter.RightHandEquipment;
        else if (bodyPartType == BodyPartType.LeftHand)
            return PlayerCharacter.LeftHandEquipment;
        else
            return null; // 他のボディパーツは未実装
    }

    public void SetEquipmentByBodyPart(BodyPartType bodyPartType, Equipment equipment)
    {
        if (bodyPartType == BodyPartType.None) return;
        Equipment currentEquipment = GetEquipmentByBodyPart(bodyPartType);
        if (currentEquipment != null)
        {
            AddItemToBag(currentEquipment.Clone());
        }

        if (bodyPartType == BodyPartType.RightHand)
            PlayerCharacter.RightHandEquipment = equipment.Clone() as Equipment;
        else if (bodyPartType == BodyPartType.LeftHand)
            PlayerCharacter.LeftHandEquipment = equipment.Clone() as Equipment;

        PlayerCharacter.ColStatus();
    }

    /// <summary>
    /// 装備を削除
    /// </summary>
    public void RemoveEquipmentbyBodyPart(BodyPartType bodyPartType)
    {
        if (bodyPartType == BodyPartType.None) return;

        if (bodyPartType == BodyPartType.RightHand)
            PlayerCharacter.RightHandEquipment = null;
        else if (bodyPartType == BodyPartType.LeftHand)
            PlayerCharacter.LeftHandEquipment = null;

        PlayerCharacter.ColStatus();
    }

    /// <summary>
    /// バッグからアイテムを削除
    /// </summary>
    public void RemoveItemFromBag(Item item)
    {
        if (item == null) return;
        PlayerCharacter.RemoveBagItem(item);
    }

    /// <summary>
    /// ポケットからアイテムを削除
    /// </summary>
    public void RemoveItemFromPocket(Item item)
    {
        if (item == null) return;

        if (item is Consumable consumable)
        {
            PlayerCharacter.PocketList.Remove(consumable);
        }
    }

    public void SellItem(Item item)
    {
        if (item == null) return;

        float discountRate = 0.5f; // 50%の割引率

        // アイテムの価格を取得
        int coinPrice = (int)(item.Base.CoinPrice * discountRate);
        int discPrice = (int)(item.Base.DiscPrice * discountRate);

        // プレイヤーにコインとディスクを追加
        AddCoin(coinPrice);
        AddDisc(discPrice);
    }

    /// <summary>
    /// アイテムをドロップ
    /// </summary>
    public void DropItem(Item item)
    {
        if (item == null) return;
        FieldController.Instance.DropPlayerItem(item);
    }

    public void PickUpItem(Item item)
    {
        messagePanel.AddMessage(MessageIconType.Bag, $"{item.Base.Name}をひろった");
        AddItemToBag(item);
    }

    #endregion

    #region Command Management
    /// <summary>
    /// コマンドをストレージに追加
    /// </summary>
    public void AddCommandToStorage(Command command)
    {
        if (command == null) return;

        if (PlayerCharacter.ColStorage <= PlayerCharacter.StorageList.Count)
        {
            Debug.LogWarning("ストレージの容量を超えています。コマンドを追加できません。");
            FieldController.Instance.DropPlayerCommand(command);
            return;
        }
        PlayerCharacter.AddCommandToStorage(command.Clone());
    }

    /// <summary>
    /// ストレージからコマンドを削除
    /// </summary>
    public void RemoveCommandFromStorage(Command command)
    {
        Debug.Log("RemoveCommandFromStorage");
        if (command == null) return;

        Debug.Log(PlayerCharacter.StorageList.Count);

        if (PlayerCharacter.StorageList.Contains(command))
        {
            PlayerCharacter.StorageList.Remove(command);
            Debug.Log($"ストレージからコマンドを削除しました: {command.Base.Name}");
        }
        else
        {
            Debug.LogWarning("ストレージに指定のコマンドが存在しません。");
        }
    }

    /// <summary>
    /// コマンドをスロットに追加
    /// </summary>
    public void AddCommandToSlot(Command command, int index)
    {
        if (!IsValidSlotIndex(index))
        {
            HandleInvalidSlotIndex(command);
            return;
        }

        PlayerCharacter.SlotList[index] = command.Clone();
    }

    /// <summary>
    /// スロットからコマンドを削除
    /// </summary>
    public void RemoveCommandFromSlot(int index)
    {
        if (!IsValidSlotIndex(index))
        {
            Debug.LogWarning("無効なインデックスです。コマンドを削除できません。");
            return;
        }

        PlayerCharacter.SlotList[index] = null;
    }

    public void SellCommand(Command command)
    {
        if (command == null) return;

        float discountRate = 0.5f; // 50%の割引率

        // コマンドの価格を取得
        int coinPrice = (int)(command.Base.CoinPrice * discountRate);
        int discPrice = (int)(command.Base.DiscPrice * discountRate);

        // プレイヤーにコインとディスクを追加
        AddCoin(coinPrice);
        AddDisc(discPrice);
    }

    /// <summary>
    /// コマンドをドロップ
    /// </summary>
    public void DropCommand(Command command)
    {
        if (command == null) return;
        FieldController.Instance.DropPlayerCommand(command);
    }

    private bool IsValidSlotIndex(int index)
    {
        return index >= 0 && index < PlayerCharacter.ColMemory * 3;
    }

    private void HandleInvalidSlotIndex(Command command)
    {
        FieldController.Instance.DropPlayerCommand(command);
        Debug.LogWarning("無効なインデックスです。コマンドを追加できません。");
    }
    #endregion

    #region Currency Management
    /// <summary>
    /// お金を追加
    /// </summary>
    public void AddCoin(int amount)
    {
        PlayerCharacter.Coin += amount;
        UpdateCurrencyPanel();
    }

    /// <summary>
    /// ディスクを追加
    /// </summary>
    public void AddDisc(int amount)
    {
        PlayerCharacter.Disc += amount;
        UpdateCurrencyPanel();
    }

    /// <summary>
    /// キーを追加
    /// </summary>
    public void AddKey(int amount)
    {
        PlayerCharacter.Key += amount;
        UpdateCurrencyPanel();
    }

    public bool SpendCurrency(int coin, int disc = 0)
    {
        if (PlayerCharacter.Coin < coin || PlayerCharacter.Disc < disc)
        {
            Debug.LogWarning("お金またはディスクが不足しています。");
            return false;
        }

        PlayerCharacter.Coin -= coin;
        PlayerCharacter.Disc -= disc;
        UpdateCurrencyPanel();
        return true;
    }

    /// <summary>
    /// キーを消費
    /// </summary>
    public bool SpendKey(int amount)
    {
        if (PlayerCharacter.Key < amount) return false;

        PlayerCharacter.Key -= amount;
        UpdateCurrencyPanel();
        return true;
    }
    #endregion

    #region UI Management
    /// <summary>
    /// 通貨パネルを更新
    /// </summary>
    private void UpdateCurrencyPanel()
    {
        currencyPanel.SetCoin(PlayerCharacter.Coin);
        currencyPanel.SetDisc(PlayerCharacter.Disc);
        currencyPanel.SetKey(PlayerCharacter.Key);
    }

    /// <summary>
    /// プレイヤーエネルギーを更新
    /// </summary>
    public void UpdatePlayerEnergy()
    {
        StartCoroutine(playerSubPanel.UpdateEnergyGauges());
    }
    #endregion

    #region Energy Management
    public bool IsAlive()
    {
        return PlayerCharacter.Life > 0;
    }

    /// <summary>
    /// エネルギーコストをチェック
    /// </summary>
    public bool CheckEnergyCost(List<EnergyCost> energyCosts)
    {
        if (energyCosts == null || energyCosts.Count == 0)
        {
            Debug.LogWarning("エネルギーコストが設定されていません。");
            return false;
        }

        foreach (EnergyCost energyCost in energyCosts)
        {
            int currentVal = GetEnergyValue(energyCost.type);
            if (currentVal <= energyCost.val)
            {
                Debug.LogWarning($"{energyCost.type} が不足しています。");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// エネルギーコストを消費
    /// </summary>
    public bool UseEnergyCost(List<EnergyCost> energyCosts)
    {
        if (!CheckEnergyCost(energyCosts)) return false;

        foreach (EnergyCost energyCost in energyCosts)
        {
            CalculationEnergy(energyCost.type, energyCost.val);
        }

        UpdatePlayerEnergy();
        return true;
    }

    /// <summary>
    /// エネルギー値を取得
    /// </summary>
    private int GetEnergyValue(EnergyType type)
    {
        return type switch
        {
            EnergyType.Life => PlayerCharacter.Life,
            EnergyType.Battery => PlayerCharacter.Battery,
            EnergyType.Soul => PlayerCharacter.Soul,
            _ => 0,
        };
    }

    /// <summary>
    /// エネルギーを減少
    /// </summary>
    private void CalculationEnergy(EnergyType type, int amount)
    {
        switch (type)
        {
            case EnergyType.Life:
                PlayerCharacter.Life = Mathf.Clamp(PlayerCharacter.Life - amount, 0, PlayerCharacter.MaxLife);
                break;
            case EnergyType.Battery:
                PlayerCharacter.Battery = Mathf.Clamp(PlayerCharacter.Battery - amount, 0, PlayerCharacter.MaxBattery);
                break;
            case EnergyType.Soul:
                PlayerCharacter.Soul = Mathf.Clamp(PlayerCharacter.Soul - amount, 0, 100);
                break;
        }
    }

    public void StatusUp(StatusType type)
    {
        PlayerCharacter.StatusUp(type);
        if (PlayerCharacter.SkillPoint <= 0)
        {
            playerSubPanel.SetStatusText(null);
        }
    }

    public void TakeAttack(TotalAttackCount totalCount)
    {
        PlayerCharacter.TakeAttack(totalCount);
        UpdatePlayerEnergy();
    }

    public void TakeGuard(TotalAttackCount totalCount)
    {
        for (int i = 0; i < totalCount.EnergyAttackList.Count; i++)
        {
            EnergyCount energyCount = totalCount.EnergyAttackList[i];
            int guardVal = Mathf.Max(0, (int)(energyCount.val * energyCount.times));
            if (energyCount.type == EnergyType.Life)
            {
                PlayerCharacter.LifeGuard += guardVal;
            }
            else if (energyCount.type == EnergyType.Battery)
            {
                PlayerCharacter.BatteryGuard += guardVal;
            }
        }
        UpdatePlayerEnergy();
    }

    public void ClearGuard()
    {
        PlayerCharacter.LifeGuard = 0;
        PlayerCharacter.BatteryGuard = 0;
        UpdatePlayerEnergy();
    }
    #endregion

    #region FieldPlayer
    public void SetFieldPlayerMove(bool canMove)
    {
        fieldPlayer.SetCanMove(canMove);
    }
    #endregion
}
