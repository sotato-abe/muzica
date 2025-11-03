using UnityEngine;

public enum InformationType
{
    Story, // ストーリー
    News, // ニュース
}

public static class InformationTypeExtensions
{
    public static Color GetInformationTypeColor(this InformationType informationType)
    {
        return informationType switch
        {
            InformationType.Story => new Color(255 / 255f, 215 / 255f, 0 / 255f),
            InformationType.News => new Color(34 / 255f, 139 / 255f, 34 / 255f),
            _ => Color.clear
        };
    }
}
