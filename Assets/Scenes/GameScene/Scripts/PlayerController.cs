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
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private PropertyPanel propertyPanel;
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
        UpdatePropertyPanel();
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

    #region Player Communication
    public void SetPlayerBlowing(TalkMessage talkMessage)
    {
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage));
    }
    #endregion

    #region Battle System
    /// <summary>
    /// バトル報酬を追加
    /// </summary>
    public void AddBattleReward(int exp, int money, List<Item> items)
    {
        player.AddExp(exp);
        player.AddMoney(money);

        foreach (Item item in items)
        {
            if (item != null)
            {
                AddItemToBag(item);
            }
        }

        UpdatePropertyPanel();
    }
    #endregion

    #region Inventory Management
    /// <summary>
    /// アイテムをバッグに追加
    /// </summary>
    public void AddItemToBag(Item item)
    {
        if (!CanAddToBag())
        {
            HandleBagOverflow(item);
            return;
        }
        player.AddItemToBag(item);
    }

    /// <summary>
    /// アイテムをポケットに追加
    /// </summary>
    public void AddItemToPocket(Item item)
    {
        if (!CanAddToPocket())
        {
            HandlePocketOverflow(item);
            return;
        }

        if (item is Consumable consumable)
        {
            player.AddItemToPocket(consumable);
        }
    }

    /// <summary>
    /// 装備を追加
    /// </summary>
    public void AddItemToEquip(Item item)
    {
        if (item == null) return;

        if (item is Equipment equipment)
        {
            player.EquipmentList.Add(equipment);
        }
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

    /// <summary>
    /// 装備を削除
    /// </summary>
    public void RemoveItemFromEquip(Item item)
    {
        if (item == null) return;

        if (item is Equipment equipment)
        {
            player.EquipmentList.Remove(equipment);
        }
    }

    /// <summary>
    /// アイテムをドロップ
    /// </summary>
    public void DropItem(Item item)
    {
        if (item == null) return;
        FieldController.Instance.DropPlayerItem(item);
    }

    private bool CanAddToBag()
    {
        return player.BagItemList.Count < player.Bag;
    }

    private bool CanAddToPocket()
    {
        return player.PocketList.Count < player.ColPocket;
    }

    private void HandleBagOverflow(Item item)
    {
        Debug.LogWarning("バッグの容量を超えています。アイテムを追加できません。");
        FieldController.Instance.DropPlayerItem(item);
    }

    private void HandlePocketOverflow(Item item)
    {
        Debug.LogWarning("ポケットの容量を超えています。アイテムを追加できません。");
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

        if (!CanAddToStorage())
        {
            HandleStorageOverflow(command);
            return;
        }

        player.StorageList.Add(command);
    }

    /// <summary>
    /// ストレージからコマンドを削除
    /// </summary>
    public void RemoveCommandFromStorage(Command command)
    {
        if (command == null) return;

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
    /// コマンドをテーブルに追加
    /// </summary>
    public void AddCommandToTable(Command? command, int index)
    {
        if (!IsValidTableIndex(index))
        {
            HandleInvalidTableIndex(command);
            return;
        }

        player.TableList[index] = command;
    }

    /// <summary>
    /// テーブルからコマンドを削除
    /// </summary>
    public void RemoveCommandFromTable(int index)
    {
        if (!IsValidTableIndex(index))
        {
            Debug.LogWarning("無効なインデックスです。コマンドを削除できません。");
            return;
        }

        player.TableList[index] = null;
    }

    /// <summary>
    /// コマンドをドロップ
    /// </summary>
    public void DropCommand(Command command)
    {
        if (command == null) return;
        FieldController.Instance.DropPlayerCommand(command);
    }

    private bool CanAddToStorage()
    {
        return player.StorageList.Count < player.ColStorage;
    }

    private bool IsValidTableIndex(int index)
    {
        return index >= 0 && index < player.ColMemory * 3;
    }

    private void HandleStorageOverflow(Command command)
    {
        FieldController.Instance.DropPlayerCommand(command);
        Debug.LogWarning("ストレージの容量を超えています。コマンドを追加できません。");
    }

    private void HandleInvalidTableIndex(Command command)
    {
        FieldController.Instance.DropPlayerCommand(command);
        Debug.LogWarning("無効なインデックスです。コマンドを追加できません。");
    }
    #endregion

    #region Currency Management
    /// <summary>
    /// お金を追加
    /// </summary>
    public void AddMoney(int amount)
    {
        player.Money += amount;
        UpdatePropertyPanel();
    }

    /// <summary>
    /// ディスクを追加
    /// </summary>
    public void AddDisk(int amount)
    {
        player.Disk += amount;
        UpdatePropertyPanel();
    }

    /// <summary>
    /// キーを追加
    /// </summary>
    public void AddKey(int amount)
    {
        player.Key += amount;
        UpdatePropertyPanel();
    }

    /// <summary>
    /// お金を消費
    /// </summary>
    public bool SpendMoney(int amount)
    {
        if (player.Money < amount) return false;

        player.Money -= amount;
        UpdatePropertyPanel();
        return true;
    }

    /// <summary>
    /// ディスクを消費
    /// </summary>
    public bool SpendDisk(int amount)
    {
        if (player.Disk < amount) return false;

        player.Disk -= amount;
        UpdatePropertyPanel();
        return true;
    }

    /// <summary>
    /// キーを消費
    /// </summary>
    public bool SpendKey(int amount)
    {
        if (player.Key < amount) return false;

        player.Key -= amount;
        UpdatePropertyPanel();
        return true;
    }
    #endregion

    #region UI Management
    /// <summary>
    /// プロパティパネルを更新
    /// </summary>
    private void UpdatePropertyPanel()
    {
        propertyPanel.SetMoney(player.Money);
        propertyPanel.SetDisk(player.Disk);
        propertyPanel.SetKey(player.Key);
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
        if (player.SkillPoint > 0)
        {
            switch (type)
            {
                case StatusType.LIFE:
                    player.MaxLife += 10;
                    break;
                case StatusType.BTRY:
                    player.MaxBattery += 10;
                    break;
                case StatusType.POW:
                    player.Power += 1;
                    break;
                case StatusType.TEC:
                    player.Technique += 1;
                    break;
                case StatusType.DEF:
                    player.Defense += 1;
                    break;
                case StatusType.SPD:
                    player.Speed += 1;
                    break;
                case StatusType.LUK:
                    player.Luck += 1;
                    break;
                case StatusType.MMR:
                    player.Memory += 1;
                    break;
                case StatusType.STG:
                    player.Storage += 1;
                    break;
                case StatusType.POC:
                    player.Pocket += 1;
                    break;
            }
            player.SkillPoint -= 1;
            player.CoLStatus();
            playerSubPanel.SetEnergy();
            playerSubPanel.UpdateEnergyGauges2();
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
    }

    public void TakeAttack(TotalAttackCount totalCount)
    {
        player.TakeAttack(totalCount);
        UpdatePlayerEnergy();
    }
    #endregion
}
