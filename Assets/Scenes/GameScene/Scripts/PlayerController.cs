using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] PropertyPanel propertyPanel;
    public PlayerCharacter PlayerCharacter => player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        player.Init();  // プレイヤーキャラクターの初期化
        playerSubPanel.SetCharacter(player);
        UpdatePropertyPanel();
        GameStart();
    }

    private void GameStart()
    {
        // yield return new WaitForSeconds(1f);
        TalkMessage startMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "はじめるか、、");
        StartCoroutine(playerSubPanel.SetTalkMessage(startMessage));
    }

    public void SetPlayerBlowing(TalkMessage talkMessage)
    {
        playerSubPanel.SetTalkMessage(talkMessage);
    }

    public void AddBattleReword(int exp, int money, List<Item> items)
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

    public void AddItemToBag(Item item)
    {
        // バッグの容量を超える場合はアイテムを入手しない
        if (player.BagItemList.Count >= player.Bag)
        {
            Debug.LogWarning("バッグの容量を超えています。アイテムを追加できません。");
            FieldController.Instance.DropPlayerItem(item);
            return;
        }
        player.AddItemToBag(item);
    }
    public void AddItemToPocket(Item item)
    {
        // バッグの容量を超える場合はアイテムを入手しない
        if (player.PocketList.Count >= player.ColPocket)
        {
            Debug.LogWarning("バッグの容量を超えています。アイテムを追加できません。");
            FieldController.Instance.DropPlayerItem(item);
            return;
        }
        if (item is Consumable consumable)
        {
            player.AddItemToPocket(consumable);
        }
    }

    public void AddItemToEquip(Item item)
    {
        if (item == null) return;
        if (item is Equipment equipment)
        {
            player.EquipmentList.Add(equipment);
        }
        else
        {
            Debug.LogWarning("装備アイテム以外は装備できません。");
        }
    }

    public void RemoveItemFromBag(Item item)
    {
        if (item == null) return;
        player.RemoveBagItem(item);
        Debug.Log($"バッグからアイテムを削除しました: {item.Base.Name}");
    }

    public void RemoveItemFromPocket(Item item)
    {
        if (item == null) return;
        if (item is Consumable consumable)
        {
            player.PocketList.Remove(consumable);
            Debug.Log($"ポケットからアイテムを削除しました: {consumable.Base.Name}");
        }
        else
        {
            Debug.LogWarning("消費アイテム以外は削除できません。");
        }
    }

    public void RemoveItemFromEquip(Item item)
    {
        if (item == null) return;
        if (item is Equipment equipment)
        {
            player.EquipmentList.Remove(equipment);
            Debug.Log($"装備からアイテムを削除しました: {equipment.Base.Name}");
        }
        else
        {
            Debug.LogWarning("装備アイテム以外は削除できません。");
        }
    }

    public void DropItem(Item item)
    {
        if (item == null) return;
        FieldController.Instance.DropPlayerItem(item);
    }

    public void AddCommandToStorage(Command command)
    {
        if (command == null) return;
        if (player.StorageList.Count >= player.ColStorage)
        {
            FieldController.Instance.DropPlayerCommand(command);
            Debug.LogWarning("ストレージの容量を超えています。コマンドを追加できません。");
            return;
        }
        player.StorageList.Add(command);
    }

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

    public void AddCommandToTable(Command? command, int index)
    {
        if (index >= player.ColMemory * 3)
        {
            FieldController.Instance.DropPlayerCommand(command);
            Debug.LogWarning("テーブルの容量を超えています。コマンドを追加できません。");
            return;
        }
        player.TableList[index] = command;
    }

    public void RemoveCommandFromTable(int index)
    {
        if (index < 0 || index >= player.ColMemory * 3)
        {
            Debug.LogWarning("無効なインデックスです。コマンドを削除できません。");
            return;
        }
        player.TableList[index] = null;
    }

    public void DropCommand(Command command)
    {
        if (command == null) return;
        FieldController.Instance.DropPlayerCommand(command);
    }

    public void AddMoney(int amount)
    {
        player.Money += amount;
        UpdatePropertyPanel();
        Debug.Log($"プレイヤーの所持金が増加しました: {amount} (現在の所持金: {player.Money})");
    }

    public void AddDisk(int amount)
    {
        player.Disk += amount;
        UpdatePropertyPanel();
        Debug.Log($"プレイヤーのディスクが増加しました: {amount} (現在のディスク: {player.Disk})");
    }

    public void AddKey(int amount)
    {
        player.Key += amount;
        UpdatePropertyPanel();
        Debug.Log($"プレイヤーのキーが増加しました: {amount} (現在のキー: {player.Key})");
    }

    public bool SpendMoney(int amount)
    {
        if (player.Money >= amount)
        {
            player.Money -= amount;
            Debug.Log($"プレイヤーの所持金が減少しました: {amount} (現在の所持金: {player.Money})");
            UpdatePropertyPanel();
            return true;
        }
        else
        {
            Debug.LogWarning($"所持金が不足しています。:{amount} (現在の所持金: {player.Money})");
            return false;
        }
    }

    public bool SpendDisk(int amount)
    {
        if (player.Disk >= amount)
        {
            player.Disk -= amount;
            Debug.Log($"プレイヤーのディスクが減少しました: {amount} (現在のディスク: {player.Disk})");
            UpdatePropertyPanel();
            return true;
        }
        else
        {
            Debug.LogWarning($"ディスクが不足しています。:{amount} (現在のディスク: {player.Disk})");
            return false;
        }
    }

    public bool SpendKey(int amount)
    {
        if (player.Key >= amount)
        {
            player.Key -= amount;
            Debug.Log($"プレイヤーのキーが減少しました: {amount} (現在のキー: {player.Key})");
            UpdatePropertyPanel();
            return true;
        }
        else
        {
            Debug.LogWarning($"キーが不足しています。:{amount} (現在のキー: {player.Key})");
            return false;
        }
    }

    private void UpdatePropertyPanel()
    {
        propertyPanel.SetMoney(player.Money);
        propertyPanel.SetDisk(player.Disk);
        propertyPanel.SetKey(player.Key);
    }

    public void UpdatePlayerEnergy()
    {
        playerSubPanel.SetEnergy();
    }
}
