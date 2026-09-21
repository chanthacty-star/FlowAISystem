namespace FlowAISystem.Shared.DTOs.AI;

public class StudentAIResponseDto // that is the pakage that carries the student AI's final result from application layer toward UI
{
    // AI answer
    public string Answer { get; set; }
        = string.Empty;

    public string Source { get; set; }
        = string.Empty;

    // Related lesson
    public int? LessonId { get; set; }

    public string? ActivityType { get; set; }

    // Confidence score
    public decimal Confidence { get; set; }
    public DateTime CreatedAt { get; set; }
    
}

