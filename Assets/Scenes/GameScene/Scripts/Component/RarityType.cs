using UnityEngine;

public enum RarityType
{
    Common, // コモン：６０％
    Rare, // レア：２０％
    Epic, // エピック：６％
    Legendary, // レジェンダリー：３％
    Mythic, // ミシック：0.5％
}

// レアリティによっての確率を取得
// 例：RarityType.Epic.GetProbability(); // 6.0
public static class RarityTypeExtensions
{
    public static double GetProbability(this RarityType rarity)
    {
        return rarity switch
        {
            RarityType.Common => 60.0,
            RarityType.Rare => 20.0,
            RarityType.Epic => 6.0,
            RarityType.Legendary => 3.0,
            RarityType.Mythic => 0.5,
            _ => 0.0
        };
    }
    public static Color GetRarityColor(this RarityType rarity)
    {
        return rarity switch
        {
            RarityType.Common => new Color(138 / 255f, 138 / 255f, 138 / 255f), // グレー
            RarityType.Rare => new Color(109 / 255f, 149 / 255f, 255 / 255f), // 青
            RarityType.Epic => new Color(120 / 255f, 109 / 255f, 255 / 255f), // 紫
            RarityType.Legendary => new Color(238 / 255f, 23 / 255f, 152 / 255f), // 赤
            RarityType.Mythic => new Color(255 / 255f, 178 / 255f, 28 / 255f), // 金
            _ => Color.white // デフォルトは白
        };
    }
}

