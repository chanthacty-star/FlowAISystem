using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
using FlowAISystem.Application.AI.Student.Conversation.Models;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class ConversationContextBuilder
    : IConversationContextBuilder
{
    private readonly IConversationHistoryService _historyService;

    private const int RecentMessageLimit = 10;

    public ConversationContextBuilder(
        IConversationHistoryService historyService)
    {
        _historyService = historyService;
    }


    public async Task<ConversationContext> BuildAsync(
        int conversationId,
        string currentQuestion)
    {
        var messages =
            await _historyService
                .GetMessagesAsync(conversationId);


        if (messages == null || !messages.Any())
        {
            return new ConversationContext
            {
                CurrentQuestion =
                    currentQuestion
            };
        }
        // object recent Message
        var recentMessage =
            messages
                .OrderBy(x => x.CreatedAt)
                .TakeLast(RecentMessageLimit)
                .ToList();


        // Get the latest user message
        // from the existing conversation.
        var previousUserMessage =
            messages
                .Where(x =>
                    x.Role.Equals(
                        "User",
                        StringComparison.OrdinalIgnoreCase))
                .LastOrDefault();


        // Get the latest assistant message
        // from the existing conversation.
        var previousAssistantMessage =
            messages
                .Where(x =>
                    x.Role.Equals(
                        "Assistant",
                        StringComparison.OrdinalIgnoreCase))
                .LastOrDefault();


        return new ConversationContext
        {
            CurrentQuestion =
                currentQuestion,

            PreviousUserMessage =
                previousUserMessage,

            PreviousAssistantMessage =
                previousAssistantMessage,
            RecentMessage =
                recentMessage
        };
    }
}
