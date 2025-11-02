public enum QuestActionType
{
        Talk,
        Quit,
}

public static class QuestActionTypeExtensions
{
    public static string GetActionText(this QuestActionType actionType)
    {
        return actionType switch
        {
            QuestActionType.Talk => "Talk",
            QuestActionType.Quit => "Quit",
            _ => "Unknown"
        };
    }
}