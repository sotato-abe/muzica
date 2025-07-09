using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    [SerializeField] private CharacterSubPanel playerSubPanel;
    [SerializeField] private PlayerCharacter player;
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
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1f);
        TalkMessage startMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "はじめるか、、");
        StartCoroutine(playerSubPanel.SetTalkMessage(startMessage));
    }

    public void SetPlayerBlowing(TalkMessage talkMessage)
    {
        playerSubPanel.SetTalkMessage(talkMessage);
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

    public void AddCommandToTable(Command command)
    {
        if (command == null) return;
        if (player.TableList.Count >= player.ColMemory)
        {
            FieldController.Instance.DropPlayerCommand(command);
            Debug.LogWarning("テーブルの容量を超えています。コマンドを追加できません。");
            return;
        }
        player.TableList.Add(command);
    }

    public void RemoveCommandFromTable(Command command)
    {
        if (command == null) return;
        if (player.TableList.Contains(command))
        {
            player.TableList.Remove(command);
            Debug.Log($"テーブルからコマンドを削除しました: {command.Base.Name}");
        }
        else
        {
            Debug.LogWarning("テーブルに指定のコマンドが存在しません。");
        }
    }

    public void DropCommand(Command command)
    {
        if (command == null) return;
        FieldController.Instance.DropPlayerCommand(command);
    }
}
