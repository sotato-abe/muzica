using UnityEngine;

public enum ItemType
{
    Consumable, // 消耗品(ポーチに入れることができる)
    Equipment,  // 装備品(装備することができる)
    Treasure,  // お宝（取引やクエストに使える）
}

public static class ItemTypeExtensions
{
    public static Color GetItemTypeColor(this ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Consumable => new Color(61 / 255f, 167 / 255f, 219 / 255f),
            ItemType.Equipment => new Color(80 / 255f, 80 / 255f, 80 / 255f),
            ItemType.Treasure => new Color(219 / 255f, 166 / 255f, 61 / 255f),
            _ => Color.clear
        };
    }
}