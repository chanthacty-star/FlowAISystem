using FlowAISystem.Application.AI.Knowledge.Models;

namespace FlowAISystem.Application.AI.Knowledge.Services;

public interface IRequestAnalyzer
{
    KnowledgeQuery Analyze(string userMessage, int? userId);
}