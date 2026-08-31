namespace FlowAISystem.Application.AI.Teacher.TeacherAssistant.Models;

public class TeacherAssistantRequest
{
    public string Message { get; set; } = string.Empty;

    public int? LessonId { get; set; }
}