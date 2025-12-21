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
    float paddingWidth = 60f; // 横幅

    List<CommandSlot> commandSlots = new List<CommandSlot>();

    private void Awake()
    {
        playerController = PlayerController.Instance;
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not initialized.");
            return;
        }

        slotHeight = backRectTransform.sizeDelta.y;
        // コマンドスロットを初期化
        InitializeCommandSlot();
        DisplayCommandSlot();
        SetWindowSize();
    }

    private void OnEnable()
    {
        DisplayCommandSlot();
    }

    private void SetWindowSize()
    {
        // スロットのサイズを設定
        float windowWidth = playerController.PlayerCharacter.ColMemory * 65 + paddingWidth;
        backRectTransform.sizeDelta = new Vector2(windowWidth, slotHeight);
    }

    private void InitializeCommandSlot()
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
            commandSlots.Add(newSlot);
        }
    }

    public void DisplayCommandSlot()
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
}
