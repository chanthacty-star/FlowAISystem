using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Domain.Entities.AI;
using FlowAISystem.Shared.DTOs.AI;


namespace FlowAISystem.Application.AI.Services;


public class AIConversationService
    : IAIConversationService
{

    private readonly IAIConversationRepository _repository;

    private readonly IConversationTitleService _titleService;



    public AIConversationService(
        IAIConversationRepository repository,
        IConversationTitleService titleService)
    {

        _repository = repository;

        _titleService = titleService;

    }

    public async Task<int>
        CreateConversationAsync(
            int userId)
    {

        var conversation =
            new AIConversation
            {
                UserId = userId,

                Title = "New Conversation",

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };



        await _repository
            .CreateAsync(conversation);



        return conversation.Id;

    }

    public async Task<List<AIConversationDto>>
       GetUserConversationsAsync(int userId)
    {
        var conversations =
            await _repository
                .GetByUserIdAsync(userId);

        return conversations
            .Select(x => new AIConversationDto
            {
                Id = x.Id,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                MessageCount = x.Messages.Count
            })
            .ToList();
    }

    public async Task<AIConversationDto?>
        GetConversationAsync(
            int conversationId)
    {

        var conversation =
            await _repository
            .GetByIdAsync(conversationId);



        if (conversation == null)
            return null;



        return new AIConversationDto
        {
            Id = conversation.Id,

            Title = conversation.Title,

            CreatedAt = conversation.CreatedAt,

            UpdatedAt = conversation.UpdatedAt,

            Messages =
                conversation.Messages
                .Select(m =>
                    new AIMessageDto
                    {
                        Id = m.Id,

                        Role = m.Role,

                        Content = m.Content,

                        CreatedAt = m.CreatedAt
                    })
                .ToList()
        };

    }

    public async Task AddMessageAsync(
       int conversationId,
       string role,
       string content)
    {
        // =========================================================
        // Validate
        // =========================================================

        if (conversationId <= 0)
            return;

        if (string.IsNullOrWhiteSpace(content))
            return;


        // =========================================================
        // Get Conversation
        // =========================================================

        var conversation =
            await _repository
                .GetByIdAsync(conversationId);

        if (conversation == null)
            return;


        // =========================================================
        // Create Message
        // =========================================================

        var message =
            new AIMessage
            {
                ConversationId = conversationId,

                Role = role,

                Content = content.Trim(),

                CreatedAt = DateTime.UtcNow
            };


        // =========================================================
        // Add Message
        // =========================================================

        await _repository
            .AddMessageAsync(message);


        // =========================================================
        // Update Conversation Activity
        // =========================================================

        conversation.UpdatedAt =
            DateTime.UtcNow;


        // =========================================================
        // Auto Title
        // =========================================================

        if (role.Equals(
                "User",
                StringComparison.OrdinalIgnoreCase) &&
            conversation.Title == "New Conversation")
        {
            conversation.Title =
                _titleService
                    .GenerateTitle(content);
        }


        // =========================================================
        // Save Message + Conversation
        // =========================================================

        await _repository
            .UpdateAsync(conversation);
    }

    public async Task<List<AIMessageDto>>
        GetMessagesAsync(
            int conversationId)
    {

        var messages =
            await _repository
            .GetMessagesAsync(conversationId);



        return messages

            .Select(x =>
                new AIMessageDto
                {
                    Id = x.Id,

                    Role = x.Role,

                    Content = x.Content,

                    CreatedAt = x.CreatedAt
                })

            .ToList();

    }

    public async Task UpdateConversationTitleAsync(
        int conversationId,
        string title)
    {

        var conversation =
            await _repository
            .GetByIdAsync(conversationId);



        if (conversation == null)
            return;



        conversation.Title =
            title;



        conversation.UpdatedAt =
            DateTime.UtcNow;



        await _repository
            .UpdateAsync(conversation);

    }

    public async Task UpdateAsync(
    AIConversation conversation)
    {
        await _repository.UpdateAsync(conversation);
    }

    public async Task RenameConversationAsync(
    int conversationId,
    string title)
    {

        var conversation =
            await _repository.GetByIdAsync(
                conversationId);


        if (conversation == null)
            return;



        conversation.Title =
            title;


        conversation.UpdatedAt =
            DateTime.UtcNow;



        await _repository.UpdateAsync(
            conversation);

    }

    public async Task DeleteConversationAsync(
    int conversationId)
    {

        var conversation =
            await _repository.GetByIdAsync(
                conversationId);


        if (conversation == null)
            return;



        await _repository.DeleteAsync(
            conversation);

    }

    public async Task<List<AIConversationDto>>
    SearchConversationAsync(
        int userId,
        string keyword)
    {

        var conversations =
            await _repository
            .GetByUserIdAsync(userId);



        if (string.IsNullOrWhiteSpace(keyword))
        {

            return conversations
            .Select(x => new AIConversationDto
            {
                Id = x.Id,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            })
            .ToList();

        }

        return conversations
            .Where(x =>
                x.Title.Contains(
                    keyword,
                    StringComparison
                    .OrdinalIgnoreCase))
            .Select(x => new AIConversationDto
            {
                Id = x.Id,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            })
            .ToList();
    }

}