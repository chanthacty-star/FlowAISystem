using FlowAISystem.Domain.Entities.AI;


namespace FlowAISystem.Application.AI.Interfaces;


public interface IAIConversationRepository
{

    Task<AIConversation?> GetByIdAsync(
        int id);



    Task<List<AIConversation>>
        GetByUserIdAsync(
            int userId);



    Task<AIConversation> CreateAsync(
        AIConversation conversation);



    Task AddMessageAsync(
        AIMessage message);



    Task<List<AIMessage>>
        GetMessagesAsync(
            int conversationId);



    Task UpdateAsync(
        AIConversation conversation);



    Task DeleteAsync(
        AIConversation conversation);


    Task SaveAsync();

}