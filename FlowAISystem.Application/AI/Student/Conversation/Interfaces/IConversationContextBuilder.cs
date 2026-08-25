using FlowAISystem.Application.AI.Student.Conversation.Models;

namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

public interface IConversationContextBuilder
{
    Task<ConversationContext> BuildAsync(
        int conversationId,
        string currentQuestion);
}