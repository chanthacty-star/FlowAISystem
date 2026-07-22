namespace FlowAISystem.Application.AI.Interfaces;

public interface IAIIntentDetector
{
    AIIntent Detect(string message);
}