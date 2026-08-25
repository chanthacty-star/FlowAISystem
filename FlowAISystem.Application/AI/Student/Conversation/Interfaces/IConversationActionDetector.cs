using FlowAISystem.Application.AI.Student.Conversation.Enums;

namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

public interface IConversationActionDetector
{
    ConversationAction Detect(string message);
}