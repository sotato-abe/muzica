public enum TradeActionType
{
        Talk,
        Item,
        Command,
        Quit,
}

public static class TradeActionTypeExtensions
{
    public static string GetActionText(this TradeActionType actionType)
    {
        return actionType switch
        {
            TradeActionType.Talk => "Quest",
            TradeActionType.Item => "Item",
            TradeActionType.Command => "Command",
            TradeActionType.Quit => "Quit",
            _ => "Unknown"
        };
    }
}