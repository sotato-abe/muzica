using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryQuest : Quest
{
    // 親クラスの _base をキャストして使う
    public override QuestBase Base => _base;
    public DeliveryQuestBase DeliveryQuestBase => _base as DeliveryQuestBase;

    public List<Item> DeliveryItemList = new List<Item>();
    public List<Item> RewardItems = new List<Item>();

    public bool isShipping = false;

    public DeliveryQuest(DeliveryQuestBase baseData) : base(baseData)
    {
    }

    public override void Init()
    {
        SetDeliveryItems();
    }

    public override Quest Clone()
    {
        var copy = new DeliveryQuest(DeliveryQuestBase);
        return copy;
    }

    private void SetDeliveryItems()
    {
        DeliveryItemList.Clear();
        foreach (var item in DeliveryQuestBase.DeliveryItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    if (item is ConsumableBase consumableBase)
                    {
                        Consumable consumable = new Consumable(consumableBase);
                        DeliveryItemList.Add(consumable);
                    }
                    break;
                case ItemType.Equipment:
                    if (item is EquipmentBase equipmentBase)
                    {
                        Equipment equipment = new Equipment(equipmentBase);
                        DeliveryItemList.Add(equipment);
                    }
                    break;
                case ItemType.Treasure:
                    if (item is TreasureBase treasureBase)
                    {
                        Treasure treasure = new Treasure(treasureBase);
                        DeliveryItemList.Add(treasure);
                    }
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.itemType);
                    break;
            }
        }
    }
}
