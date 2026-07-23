namespace FlowAISystem.Shared.DTOs.AI;

public class StudentAIRequestDto
{
    // Student question
    public string Question { get; set; }
        = string.Empty;


    // Optional lesson/topic reference
    public int? LessonId { get; set; }


    // Student ID for personalization
    public int StudentId { get; set; }
}