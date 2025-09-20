using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplyQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public SupplyQuestBase SupplyQuestBase => _base as SupplyQuestBase;
    public List<Item> OrderItems = new List<Item>();
    public List<Item> RewardItems = new List<Item>();

    public SupplyQuest(SupplyQuestBase baseData) : base(baseData)
    {
    }

    public override void Init()
    {
        SetOrderItems();
        SetRewardItems();
    }

    public override Quest Clone()
    {
        var copy = new SupplyQuest(SupplyQuestBase);
        return copy;
    }

    private void SetOrderItems()
    {
        OrderItems.Clear();
        foreach (var item in SupplyQuestBase.SupplyItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    Consumable bagConsumable = new Consumable((ConsumableBase)item);
                    OrderItems.Add(bagConsumable);
                    break;
                case ItemType.Equipment:
                    Equipment bagEquipment = new Equipment((EquipmentBase)item);
                    OrderItems.Add(bagEquipment);
                    break;
                case ItemType.Treasure:
                    Treasure bagTreasure = new Treasure((TreasureBase)item);
                    OrderItems.Add(bagTreasure);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.itemType);
                    break;
            }
        }
    }

    private void SetRewardItems()
    {
        RewardItems.Clear();
        foreach (var item in SupplyQuestBase.RewardItemBaseList)
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
