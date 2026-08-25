//using FlowAISystem.Shared.DTOs.AI;
//namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

//public interface IConversationHandler
//{
//    Task<StudentAIResponseDto> HandleAsync(
//        StudentAIRequestDto request,
//        string language);
//}
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Conversation.Interfaces;

public interface IConversationHandler
{
    Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request);

    Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request,
        string language);
}

