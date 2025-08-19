namespace UserManagement.Data.Enums;
public enum LogType
{
    Create,
    View,
    Update,
    Delete,
}
public static class StatusExtensions
{
    public static string ToDescription(this LogType logType, string userName)
    {
        return logType switch
        {
            LogType.Create => $"{userName} has been created",
            LogType.View => $"{userName} has been viewed",
            LogType.Update => $"{userName} has been updated",
            LogType.Delete => $"{userName} has been deleted",
            _ => "Unknown"
        };
    }
}
