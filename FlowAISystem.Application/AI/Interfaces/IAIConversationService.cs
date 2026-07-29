using FlowAISystem.Domain.Entities.AI;
using FlowAISystem.Shared.DTOs.AI;


namespace FlowAISystem.Application.AI.Interfaces;


public interface IAIConversationService
{

    Task<int> CreateConversationAsync(
        int userId);



    Task<List<AIConversationDto>>
        GetUserConversationsAsync(
            int userId);



    Task<AIConversationDto?>
        GetConversationAsync(
            int conversationId);



    Task AddMessageAsync(
        int conversationId,
        string role,
        string content);



    Task<List<AIMessageDto>>
        GetMessagesAsync(
            int conversationId);



    // NEW

    Task DeleteConversationAsync(
        int conversationId);



    Task RenameConversationAsync(
        int conversationId,
        string title);



    Task<List<AIConversationDto>>
        SearchConversationAsync(
            int userId,
            string keyword);

}