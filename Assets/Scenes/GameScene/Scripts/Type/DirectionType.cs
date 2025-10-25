using UnityEngine;
public enum DirectionType
{
    Top,
    Bottom,
    Left,
    Right,
    Other
}

public static class DirectionTypeExtensions
{
    public static DirectionType GetOppositeDirection(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Top => DirectionType.Bottom,
            DirectionType.Bottom => DirectionType.Top,
            DirectionType.Left => DirectionType.Right,
            DirectionType.Right => DirectionType.Left,
            _ => DirectionType.Other
        };
    }

    public static Vector2Int GetDirectionVector2Int(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Top => Vector2Int.up,
            DirectionType.Bottom => Vector2Int.down,
            DirectionType.Left => Vector2Int.left,
            DirectionType.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }
}