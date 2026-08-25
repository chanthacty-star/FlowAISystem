using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class ConversationHistoryService
    : IConversationHistoryService
{
    private readonly IAIConversationService _conversationService;


    public ConversationHistoryService(
        IAIConversationService conversationService)
    {
        _conversationService = conversationService;
    }


    public async Task<List<AIMessageDto>> GetMessagesAsync(
        int conversationId)
    {
        if (conversationId <= 0)
        {
            return new List<AIMessageDto>();
        }


        var messages =
            await _conversationService
                .GetMessagesAsync(conversationId);


        if (messages == null || !messages.Any())
        {
            return new List<AIMessageDto>();
        }


        return messages
            .OrderBy(x => x.CreatedAt)
            .ToList();
    }
}

