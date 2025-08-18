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
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private CurrencyPanel currencyPanel;
    [SerializeField] private PlayerCharacter player;
    #endregion

    #region Properties
    public PlayerCharacter PlayerCharacter => player;
    #endregion

    #region Events
    public EventType CurrentEventType { get; private set; } = EventType.Default;
    #endregion

    #region Unity Lifecycle
    void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializePlayer();
        GameStart();
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

    private void InitializePlayer()
    {
        player.Init();
        playerSubPanel.SetCharacter(player);
        UpdateCurrencyPanel();
    }

    private void GameStart()
    {
        TalkMessage startMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "はじめるか、、");
        StartCoroutine(playerSubPanel.SetTalkMessage(startMessage));
    }
    #endregion

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
        player.AddExp(exp);
        player.AddCoin(coin);

        foreach (Item item in items)
        {
            if (item != null)
            {
                AddItemToBag(item);
            }
        }

        UpdateCurrencyPanel();
    }
    #endregion

    #region Item Management
    /// <summary>
    /// アイテムをバッグに追加
    /// </summary>
    public void AddItemToBag(Item item)
    {
        if (item == null) return;

        if (player.Bag <= player.BagItemList.Count)
        {
            Debug.LogWarning($"バックの容量を超えています。アイテムを追加できません。");
            FieldController.Instance.DropPlayerItem(item.Clone());
            return;
        }
        player.AddItemToBag(item.Clone());
    }

    /// <summary>
    /// アイテムをポケットに追加
    /// </summary>
    public void AddItemToPocket(Item item)
    {
        if (player.ColPocket <= player.PocketList.Count)
        {
            Debug.LogWarning("ポケットの容量を超えています。アイテムを追加できません。");
            FieldController.Instance.DropPlayerItem(item.Clone());
            return;
        }

        if (item is Consumable consumable)
        {
            player.AddItemToPocket(consumable.Clone() as Consumable);
        }
    }

    public Equipment GetEquipmentByBodyPart(BodyPartType bodyPartType)
    {
        if (bodyPartType == BodyPartType.None) return null;
        else if (bodyPartType == BodyPartType.RightHand)
            return player.RightHandEquipment;
        else if (bodyPartType == BodyPartType.LeftHand)
            return player.LeftHandEquipment;
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
            player.RightHandEquipment = equipment.Clone() as Equipment;
        else if (bodyPartType == BodyPartType.LeftHand)
            player.LeftHandEquipment = equipment.Clone() as Equipment;

        player.ColStatus();
    }

    /// <summary>
    /// 装備を削除
    /// </summary>
    public void RemoveEquipmentbyBodyPart(BodyPartType bodyPartType)
    {
        if (bodyPartType == BodyPartType.None) return;

        if (bodyPartType == BodyPartType.RightHand)
            player.RightHandEquipment = null;
        else if (bodyPartType == BodyPartType.LeftHand)
            player.LeftHandEquipment = null;

        player.ColStatus();
    }

    /// <summary>
    /// バッグからアイテムを削除
    /// </summary>
    public void RemoveItemFromBag(Item item)
    {
        if (item == null) return;
        player.RemoveBagItem(item);
    }

    /// <summary>
    /// ポケットからアイテムを削除
    /// </summary>
    public void RemoveItemFromPocket(Item item)
    {
        if (item == null) return;

        if (item is Consumable consumable)
        {
            player.PocketList.Remove(consumable);
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

    #endregion

    #region Command Management
    /// <summary>
    /// コマンドをストレージに追加
    /// </summary>
    public void AddCommandToStorage(Command command)
    {
        if (command == null) return;

        if (player.ColStorage <= player.StorageList.Count)
        {
            Debug.LogWarning("ストレージの容量を超えています。コマンドを追加できません。");
            FieldController.Instance.DropPlayerCommand(command);
            return;
        }
        player.AddCommandToStorage(command.Clone());
    }

    /// <summary>
    /// ストレージからコマンドを削除
    /// </summary>
    public void RemoveCommandFromStorage(Command command)
    {
        Debug.Log("RemoveCommandFromStorage");
        if (command == null) return;

        Debug.Log(player.StorageList.Count);

        if (player.StorageList.Contains(command))
        {
            player.StorageList.Remove(command);
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

        player.SlotList[index] = command.Clone();
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

        player.SlotList[index] = null;
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
        return index >= 0 && index < player.ColMemory * 3;
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
        player.Coin += amount;
        UpdateCurrencyPanel();
    }

    /// <summary>
    /// ディスクを追加
    /// </summary>
    public void AddDisc(int amount)
    {
        player.Disc += amount;
        UpdateCurrencyPanel();
    }

    /// <summary>
    /// キーを追加
    /// </summary>
    public void AddKey(int amount)
    {
        player.Key += amount;
        UpdateCurrencyPanel();
    }

    public bool SpendCurrency(int coin, int disc = 0)
    {
        if (player.Coin < coin || player.Disc < disc)
        {
            Debug.LogWarning("お金またはディスクが不足しています。");
            return false;
        }

        player.Coin -= coin;
        player.Disc -= disc;
        UpdateCurrencyPanel();
        return true;
    }

    /// <summary>
    /// キーを消費
    /// </summary>
    public bool SpendKey(int amount)
    {
        if (player.Key < amount) return false;

        player.Key -= amount;
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
        currencyPanel.SetCoin(player.Coin);
        currencyPanel.SetDisc(player.Disc);
        currencyPanel.SetKey(player.Key);
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
        return player.Life > 0;
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
            EnergyType.Life => player.Life,
            EnergyType.Battery => player.Battery,
            EnergyType.Soul => player.Soul,
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
                player.Life = Mathf.Clamp(player.Life - amount, 0, player.MaxLife);
                break;
            case EnergyType.Battery:
                player.Battery = Mathf.Clamp(player.Battery - amount, 0, player.MaxBattery);
                break;
            case EnergyType.Soul:
                player.Soul = Mathf.Clamp(player.Soul - amount, 0, 100);
                break;
        }
    }

    public void StatusUp(StatusType type)
    {
        player.StatusUp(type);
    }

    public void TakeAttack(TotalAttackCount totalCount)
    {
        player.TakeAttack(totalCount);
        UpdatePlayerEnergy();
    }
    #endregion
}
