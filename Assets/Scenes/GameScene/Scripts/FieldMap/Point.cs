using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Point
{
    [SerializeField] PointBase _base;
    public PointBase Base { get => _base; set => _base = value; }

    public Character Owner { get; set; }
    public List<Item> ShopItems { get; set; }
    public List<Command> ShopCommands { get; set; }
    public List<Quest> ShopQuests { get; set; }

    public static Point CreateFrom(PointBase baseData)
    {
        var point = new Point
        {
            Base = baseData,
            ShopItems = new List<Item>(),
            ShopCommands = new List<Command>(),
            ShopQuests = new List<Quest>(),
        };
        point.Owner = new Character(baseData.Owner); // PointBaseからオーナーを取得
        point.SetShopItems();
        point.SetShopCommands();
        point.SetShopQuests();

        return point;
    }

    public void ResetMerchandise()
    {
        ShopItems.Clear();
        ShopCommands.Clear();
        SetShopItems();
        SetShopCommands();
        SetShopQuests();
    }

    private void SetShopItems()
    {
        foreach (ItemBase item in _base.ShopItemBaseList)
        {
            switch (item.itemType)
            {
                case ItemType.Consumable:
                    Consumable consumable = new Consumable((ConsumableBase)item);
                    ShopItems.Add(consumable);
                    break;
                case ItemType.Equipment:
                    Equipment equipment = new Equipment((EquipmentBase)item);
                    ShopItems.Add(equipment);
                    break;
                case ItemType.Treasure:
                    Treasure treasure = new Treasure((TreasureBase)item);
                    ShopItems.Add(treasure);
                    break;
                default:
                    Debug.LogError("Unknown item type: " + item.itemType);
                    break;
            }
        }
    }

    private void SetShopCommands()
    {
        foreach (CommandBase commandBase in _base.ShopCommandBaseList)
        {
            Command command = new Command(commandBase);
            ShopCommands.Add(command);
        }
    }

    private void SetShopQuests()
    {
        foreach (QuestBase quest in _base.ShopQuestBaseList)
        {
            switch (quest.QuestType)
            {
                case QuestType.Delivery:
                    DeliveryQuest deliveryQuest = new DeliveryQuest((DeliveryQuestBase)quest);
                    ShopQuests.Add(deliveryQuest);
                    break;
                case QuestType.Story:
                    StoryQuest storyQuest = new StoryQuest((StoryQuestBase)quest);
                    ShopQuests.Add(storyQuest);
                    break;
                case QuestType.Supply:
                    SupplyQuest supplyQuest = new SupplyQuest((SupplyQuestBase)quest);
                    ShopQuests.Add(supplyQuest);
                    break;
                case QuestType.Extermination:
                    ExterminationQuest exterminationQuest = new ExterminationQuest((ExterminationQuestBase)quest);
                    ShopQuests.Add(exterminationQuest);
                    break;
                case QuestType.Special:
                    SpecialQuest specialQuest = new SpecialQuest((SpecialQuestBase)quest);
                    ShopQuests.Add(specialQuest);
                    break;
                default:
                    break;
            }
        }
    }

    public Quest GetActiveQuest()
    {
        if (ShopQuests == null || ShopQuests.Count == 0) return null;
        foreach (Quest quest in ShopQuests)
        {
            if (!quest.IsCompleted())
            {
                return quest;
            }
        }
        return null;
    }
}
