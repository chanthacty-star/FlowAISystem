using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

public interface IConversationHistoryService
{
    Task<List<AIMessageDto>> GetMessagesAsync(
        int conversationId);
}