using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public WorkQuestBase WorkQuestBase => _base as WorkQuestBase;
    public List<Item> RewardItems = new List<Item>();

    public WorkQuest(WorkQuestBase baseData) : base(baseData)
    {
    }

    public override void Init()
    {
        SetRewardItems();
    }

    public override Quest Clone()
    {
        var copy = new WorkQuest(WorkQuestBase);
        return copy;
    }

    private void SetRewardItems()
    {
        RewardItems.Clear();
        foreach (var item in WorkQuestBase.RewardItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    Consumable bagConsumable = new Consumable((ConsumableBase)item);
                    RewardItems.Add(bagConsumable);
                    break;
                case ItemType.Equipment:
                    Equipment bagEquipment = new Equipment((EquipmentBase)item);
                    RewardItems.Add(bagEquipment);
                    break;
                case ItemType.Treasure:
                    Treasure bagTreasure = new Treasure((TreasureBase)item);
                    RewardItems.Add(bagTreasure);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.itemType);
                    break;
            }
        }
    }
}
