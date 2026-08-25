using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Conversation.Models;

public class ConversationContext
{
    public AIMessageDto? PreviousUserMessage { get; set; }


    public AIMessageDto? PreviousAssistantMessage { get; set; }


    public string CurrentQuestion { get; set; }
        = string.Empty;


    public bool HasHistory =>
        PreviousUserMessage != null &&
        PreviousAssistantMessage != null;
}

