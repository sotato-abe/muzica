using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlotSettingWindow : MonoBehaviour
{
    [SerializeField] CommandSlot commandSlotPrefab;
    [SerializeField] GameObject slotList;
    [SerializeField] RectTransform backRectTransform;
    PlayerController playerController;

    float slotHeight; // スロットの高さ
    float paddingWidth = 50f; // 横幅

    List<CommandSlot> commandSlots = new List<CommandSlot>();
    public delegate void TargetCommandDelegate(Command? command, bool isOwn = true);
    public event TargetCommandDelegate OnTargetCommand;

    private void Start()
    {
        playerController = PlayerController.Instance;
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not initialized.");
            return;
        }

        slotHeight = backRectTransform.sizeDelta.y;
        // コマンドスロットを初期化
        InitializeCommandSlots();
        SetCommandSlot();
        SetWindowSize();
    }

    private void SetWindowSize()
    {
        // スロットのサイズを設定
        float windowWidth = playerController.PlayerCharacter.ColMemory * 100 + paddingWidth;
        backRectTransform.sizeDelta = new Vector2(windowWidth, slotHeight);
    }

    public void SetCommandSlot()
    {
        // 既存のスロットをクリア
        foreach (CommandSlot slot in commandSlots)
        {
            slot.ResetSlot();
        }

        // スロットにコマンドをセット
        for (int i = 0; i < playerController.PlayerCharacter.ColMemory * 3; i++)
        {
            Command command = playerController.PlayerCharacter.SlotList[i];
            if (command != null && command.Base != null)
            {
                commandSlots[i].SetCommand(command);
            }
        }
    }

    private void InitializeCommandSlots()
    {
        // 既存のスロットをクリア
        foreach (Transform child in slotList.transform)
        {
            Destroy(child.gameObject);
        }

        // コマンドスロットを生成
        for (int i = 0; i < playerController.PlayerCharacter.ColMemory * 3; i++)
        {
            CommandSlot newSlot = Instantiate(commandSlotPrefab, slotList.transform);
            newSlot.SlotIndex = i; // スロットのインデックスを設定            
            newSlot.OnTargetCommand += TargetCommand; // コマンド選択イベントを登録
            commandSlots.Add(newSlot);
        }
    }

    public void TargetCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null)
        {
            OnTargetCommand?.Invoke(null);
            return;
        }
        OnTargetCommand?.Invoke(commandBlock.Command);
    }
}
