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


    // =========================================================
    // Get Conversation By Id
    // =========================================================

    public async Task<AIConversation?> GetByIdAsync(
        int id)
    {
        return await _context.AIConversations
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    // =========================================================
    // Get User Conversations
    // =========================================================

    public async Task<List<AIConversation>>
        GetByUserIdAsync(int userId)
    {
        return await _context.AIConversations
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();
    }


    // =========================================================
    // Create Conversation
    // =========================================================

    public async Task<AIConversation> CreateAsync(
        AIConversation conversation)
    {
        var now = DateTime.UtcNow;

        // Make sure timestamps are initialized.
        if (conversation.CreatedAt == default)
        {
            conversation.CreatedAt = now;
        }

        conversation.UpdatedAt = now;

        await _context.AIConversations.AddAsync(
            conversation);

        await SaveChangesAsync();

        return conversation;
    }


    // =========================================================
    // Add Message
    // =========================================================

    public async Task AddMessageAsync(
        AIMessage message)
    {
        await _context.AIMessages.AddAsync(message);
    }


    // =========================================================
    // Get Messages
    // =========================================================

    public async Task<List<AIMessage>>
        GetMessagesAsync(int conversationId)
    {
        return await _context.AIMessages
            .Where(x =>
                x.ConversationId == conversationId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }


    // =========================================================
    // Update Conversation
    // =========================================================

    public async Task UpdateAsync(
        AIConversation conversation)
    {
        _context.AIConversations.Update(
            conversation);

        await SaveChangesAsync();
    }


    // =========================================================
    // Delete Conversation
    // =========================================================

    public async Task DeleteAsync(
        AIConversation conversation)
    {
        _context.AIConversations.Remove(
            conversation);

        await SaveChangesAsync();
    }


    // =========================================================
    // Save
    // =========================================================

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }


    // =========================================================
    // Internal Save
    // =========================================================

    private async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}