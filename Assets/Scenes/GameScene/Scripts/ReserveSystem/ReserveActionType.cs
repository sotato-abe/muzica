public enum ReserveActionType
{
        Bag,
        Storage,
        Status,
        Quit,
}

public static class ReserveActionTypeExtensions
{
    public static string GetActionText(this ReserveActionType actionType)
    {
        return actionType switch
        {
            ReserveActionType.Bag => "Bag",
            ReserveActionType.Storage => "Storage",
            ReserveActionType.Status => "Status",
            ReserveActionType.Quit => "Quit",
            _ => "Unknown"
        };
    }
}