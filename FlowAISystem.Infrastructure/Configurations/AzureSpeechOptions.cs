namespace FlowAISystem.Infrastructure.Configuration;

public class AzureSpeechOptions
{
    public const string SectionName = "AzureSpeech";
    public string SubscriptionKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}