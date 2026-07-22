namespace FlowAISystem.Web.Services;

public class NotificationService
{
    public event Action<NotificationMessage>? OnNotify;

    public void Show(
        NotificationType type,
        string title,
        string message,
        int duration = 4000)
    {
        OnNotify?.Invoke(new NotificationMessage
        {
            Type = type,
            Title = title,
            Message = message,
            Duration = duration
        });
    }

    public void Success(
        string message,
        string title = "Success")
    {
        Show(NotificationType.Success,
             title,
             message);
    }

    public void Error(
        string message,
        string title = "Error")
    {
        Show(NotificationType.Error,
             title,
             message);
    }

    public void Warning(
        string message,
        string title = "Warning")
    {
        Show(NotificationType.Warning,
             title,
             message);
    }

    public void Info(
        string message,
        string title = "Information")
    {
        Show(NotificationType.Information,
             title,
             message);
    }
}