using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldController : MonoBehaviour
{
    private const int ITEM_GET_PROBABILITY = 30;
    public static FieldController Instance { get; private set; }

    private FieldData currentFieldData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複防止
            return;
        }
        Instance = this;
    }

    public void SetField(FieldData fieldData)
    {
        currentFieldData = fieldData;
    }

    public void OpenTreasureBox()
    {
        if (Random.Range(0, 100f) < ITEM_GET_PROBABILITY)
        {
            Debug.LogWarning("スカ");
            return;
        }

        if (currentFieldData == null || currentFieldData.TreasureBoxItems.Count == 0)
        {
            Debug.LogWarning("No treasures available in the current field.");
            return;
        }

        // 1. 総確率を計算
        double totalWeight = currentFieldData.TreasureBoxItems.Sum(item => item.Rarity.GetProbability());

        // 2. ランダム値を生成（0〜totalWeight）
        double randomValue = Random.Range(0f, (float)totalWeight);

        // 3. 重みでアイテムを抽選
        double cumulative = 0;
        foreach (var item in currentFieldData.TreasureBoxItems)
        {
            cumulative += item.Rarity.GetProbability();
            if (randomValue <= cumulative)
            {
                Debug.Log($"You found a treasure: {item.Name} (Rarity: {item.Rarity})");
                // ここでインベントリに追加する処理を入れればOK
                return;
            }
        }

        // 予備処理（念のため）
        Debug.LogWarning("抽選失敗（このメッセージは基本出ない）");
    }
}
