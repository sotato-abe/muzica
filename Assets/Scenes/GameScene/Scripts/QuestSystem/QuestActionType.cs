public enum QuestActionType
{
        Quest,
        Quit,
}

public static class QuestActionTypeExtensions
{
    public static string GetActionText(this QuestActionType actionType)
    {
        return actionType switch
        {
            QuestActionType.Quest => "Quest",
            QuestActionType.Quit => "Quit",
            _ => "Unknown"
        };
    }
}