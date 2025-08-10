using UnityEngine;
public enum EnergyType
{
    Life,
    Battery,
    Soul,
}

public static class EnergyTypeExtensions
{
    public static Color GetEnergyColor(this EnergyType energy)
    {
        return energy switch
        {
            EnergyType.Life => new Color(255 / 255f, 101 / 255f, 0 / 255f),
            EnergyType.Battery => new Color(255 / 255f, 172 / 255f, 0 / 255f), // 青
            EnergyType.Soul => new Color(0 / 255f, 255 / 255f, 201 / 255f), // 紫
            _ => Color.white // デフォルトは白
        };
    }
}