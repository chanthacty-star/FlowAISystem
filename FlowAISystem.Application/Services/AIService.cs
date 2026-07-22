using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Application.AI;

namespace FlowAISystem.Application.Services;

public class AIService : IAIService
{
    private readonly AIRequestRouter _router;


    public AIService(
        AIRequestRouter router)
    {
        _router = router;
    }


    public async Task<string> AskAsync(
        string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return "Please enter a question.";


        return await _router.ProcessAsync(message);
    }
}

