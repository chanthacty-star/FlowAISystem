using FlowAISystem.Application.AI.Student.Conversation.Models;

namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

public interface IPromptBuilder
{
    string BuildExplainMore(ConversationContext context);

    string BuildExample(ConversationContext context);

    string BuildQuiz(ConversationContext context);

    string BuildTranslation(ConversationContext context);

    string BuildContinue(ConversationContext context);

    string BuildComparison(ConversationContext context);

    string BuildSummary(ConversationContext context);
}