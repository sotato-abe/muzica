using UnityEngine;

public enum QuestType
{
    Story, // ストーリー
    Delivery, // 運搬
    Extermination, // 討伐
    Work, // 作業
    Special // 特殊
}

public static class QuestTypeExtensions
{
    public static Color GetQuestTypeColor(this QuestType questType)
    {
        return questType switch
        {
            QuestType.Story => new Color(255 / 255f, 215 / 255f, 0 / 255f),
            QuestType.Delivery => new Color(30 / 255f, 144 / 255f, 255 / 255f),
            QuestType.Extermination => new Color(220 / 255f, 20 / 255f, 60 / 255f),
            QuestType.Work => new Color(255 / 255f, 165 / 0f, 0 / 255f),
            QuestType.Special => new Color(138 / 255f, 43 / 255f, 226 / 255f),
            _ => Color.clear
        };
    }

    public static string GetQuestTypeReceiptString(this QuestType questType)
    {
        return questType switch
        {
            QuestType.Story => "進める",
            QuestType.Delivery => "引き受ける",
            QuestType.Extermination => "討伐完了",
            QuestType.Work => "引き受ける",
            QuestType.Special => "進める",
            _ => "不明"
        };
    }
}
