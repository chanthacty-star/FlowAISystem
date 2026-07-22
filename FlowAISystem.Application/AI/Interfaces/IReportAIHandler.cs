namespace FlowAISystem.Application.AI.Interfaces;

public interface IReportAIHandler
{
    Task<string> HandleAsync(
        string question);
}