using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CommandReel : Window
{
    [SerializeField] int reelNum = 1; // リールの数
    [SerializeField] List<CommandImage> reelSlots; // Slot1〜3をアサイン
    [SerializeField] float scrollSpeed = 200f;
    private int[] currentCommandIndices;
    private bool isSpinning = true; // 回転状態を管理

    List<Command> commands = new List<Command>();
    private RectTransform[] slotRects;
    private float slotHeight;

    private int padding = 20; // スロットの間隔
    private float centerY = 60f; // 中央位置のY座標

    private void Start()
    {
        // コマンドを先に設定
        SetCommands();

        // 各スロットの高さを取得
        slotRects = new RectTransform[reelSlots.Count];
        for (int i = 0; i < reelSlots.Count; i++)
        {
            slotRects[i] = reelSlots[i].rectTransform;
        }
        slotHeight = slotRects[0].rect.height;

        currentCommandIndices = new int[slotRects.Length];
        for (int i = 0; i < currentCommandIndices.Length; i++)
        {
            currentCommandIndices[i] = i % commands.Count; // commands.Count は0でないことが保証される
        }
    }

    private void OnEnable()
    {
        isSpinning = true; // パネルが有効になったら回転を開始
        SetCommands(); // コマンドを再設定
        StartReel(); // リールを開始
    }

    private void Update()
    {
        if (isSpinning)
        {
            ScrollSlots();
        }
    }

    private void SetCommands()
    {
        commands.Clear();

        int startNum = (reelNum - 1) * 3;
        for (int i = startNum; i < reelNum * 3; i++)
        {
            Command command = PlayerController.Instance.PlayerCharacter.SlotList[i];
            commands.Add(command);
        }
        for (int i = 0; i < reelSlots.Count; i++)
        {
            reelSlots[i].SetCommand(commands[i % commands.Count]);
        }
    }

    private void ScrollSlots()
    {
        for (int i = 0; i < slotRects.Length; i++)
        {
            Vector2 pos = slotRects[i].anchoredPosition;
            pos.y -= scrollSpeed * Time.deltaTime; // 下方向へスクロール
            slotRects[i].anchoredPosition = pos;

            // 下に抜けたら上に回す
            if (pos.y < -(slotHeight * 1.5f))
            {
                pos.y += (slotHeight + padding) * reelSlots.Count;
                slotRects[i].anchoredPosition = pos;
            }
        }
    }

    public Command StopReel()
    {
        isSpinning = false; // 回転を停止

        // スムーズな移動でスロットを中央に配置
        StartCoroutine(SmoothMoveToCenter());

        CommandImage closestSlot = GetClosestSlotToCenter();
        if (closestSlot.command == null)
        {
            // SoundSystem.Instance.PlaySE(SeType.ReelMiss);
            return null;
        }

        // スロットのコマンドをリセット
        // SoundSystem.Instance.PlaySE(SeType.ReelHit);
        return closestSlot.command;
    }

    private IEnumerator SmoothMoveToCenter()
    {
        CommandImage closestSlot = null;
        float minDistance = float.MaxValue;
        int closestIndex = -1;

        // 最も近いスロットを見つける
        for (int i = 0; i < reelSlots.Count; i++)
        {
            float slotCenterY = slotRects[i].anchoredPosition.y;
            float distance = Mathf.Abs(slotCenterY - centerY);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = reelSlots[i];
                closestIndex = i;
            }
        }

        if (closestSlot != null)
        {
            float offsetY = slotRects[closestIndex].anchoredPosition.y + centerY;
            Vector2[] startPositions = new Vector2[slotRects.Length];
            Vector2[] targetPositions = new Vector2[slotRects.Length];

            // 開始位置と目標位置を記録
            for (int i = 0; i < slotRects.Length; i++)
            {
                startPositions[i] = slotRects[i].anchoredPosition;
                targetPositions[i] = startPositions[i] - new Vector2(0, offsetY);
            }

            // アニメーション実行
            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = Mathf.SmoothStep(0f, 1f, t); // スムーズな補間

                for (int i = 0; i < slotRects.Length; i++)
                {
                    slotRects[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], t);
                }

                yield return null;
            }

            // 最終位置を確実に設定
            for (int i = 0; i < slotRects.Length; i++)
            {
                slotRects[i].anchoredPosition = targetPositions[i];
                SoundSystem.Instance.PlaySE(SeType.ReelMiss);
            }
        }
    }

    public void StartReel()
    {
        isSpinning = true; // 回転を開始
    }

    // 回転を停止し、中央に最も近いスロットを取得する
    private CommandImage GetClosestSlotToCenter()
    {
        CommandImage closestSlot = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < reelSlots.Count; i++)
        {
            float distance = Mathf.Abs(slotRects[i].anchoredPosition.y + centerY);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = reelSlots[i];
            }
        }

        return closestSlot;
    }
}
