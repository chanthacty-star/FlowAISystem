namespace FlowAISystem.Shared.DTOs.AI;

public class StudentAIResponseDto
{
    // AI answer
    public string Answer { get; set; }
        = string.Empty;


    // Source of knowledge
    // Example:
    // Teacher Lesson
    // AI Knowledge Base
    // Rule Engine
    public string Source { get; set; }
        = string.Empty;


    // Related lesson
    public int? LessonId { get; set; }


    // Confidence score
    public decimal Confidence { get; set; }
    public DateTime CreatedAt { get; set; }
}

