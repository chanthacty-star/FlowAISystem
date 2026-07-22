using FlowAISystem.Shared.DTOs.Students;

namespace FlowAISystem.Application.AI.Memory;

public class AIConversationContext
{
    // ==========================================
    // Conversation
    // ==========================================

    public string LastQuestion { get; set; } =
        string.Empty;

    public string LastAnswer { get; set; } =
        string.Empty;

    // ==========================================
    // Current Student
    // ==========================================

    public StudentListItemDto? CurrentStudent { get; set; }

    // ==========================================
    // Conversation Status
    // ==========================================

    public DateTime LastInteractionUtc { get; set; } =
        DateTime.UtcNow;
}