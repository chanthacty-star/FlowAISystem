using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Domain.Entities.AI;
using FlowAISystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace FlowAISystem.Infrastructure.Repositories;


public class AIConversationRepository
    : IAIConversationRepository
{

    private readonly AppDbContext _context;


    public AIConversationRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<AIConversation?> GetByIdAsync(
        int id)
    {
        return await _context.AIConversations
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id == id);
    }



    public async Task<List<AIConversation>>
        GetByUserIdAsync(int userId)
    {
        return await _context.AIConversations
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();
    }



    public async Task<AIConversation> CreateAsync(
        AIConversation conversation)
    {
        await _context.AIConversations.AddAsync(conversation);

        await SaveChangesAsync();

        return conversation;
    }



    public async Task AddMessageAsync(
        AIMessage message)
    {
        await _context.AIMessages.AddAsync(message);
    }



    public async Task<List<AIMessage>>
        GetMessagesAsync(int conversationId)
    {
        return await _context.AIMessages
            .Where(x => x.ConversationId == conversationId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }



    public async Task UpdateAsync(
        AIConversation conversation)
    {
        _context.AIConversations.Update(conversation);

        await SaveChangesAsync();
    }



    public async Task DeleteAsync(
        AIConversation conversation)
    {
        _context.AIConversations.Remove(conversation);

        await SaveChangesAsync();
    }



    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }



    private async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}