using FlowAISystem.Application.AI.Teacher.TeacherAssistant.Models;

namespace FlowAISystem.Application.AI.Teacher.TeacherAssistant.Interfaces;

public interface ITeacherAssistant
{
    Task<TeacherAssistantResponse> AskAsync(
        TeacherAssistantRequest request);
    Task<TeacherAssistantResponse> ApplySuggestionsAsync(int lessonId); //It's calling Method ApplySuggestionsAsync
}
