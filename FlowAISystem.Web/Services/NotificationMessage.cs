namespace FlowAISystem.Web.Services;

public class NotificationMessage
{
    public NotificationType Type { get; set; }

    public string Title { get; set; } = "";

    public string Message { get; set; } = "";

    public int Duration { get; set; } = 4000;
}