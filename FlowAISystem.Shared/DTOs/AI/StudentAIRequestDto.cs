namespace FlowAISystem.Shared.DTOs.AI;

public class StudentAIRequestDto
{
    // ==========================================
    // Student Question
    // ==========================================
    public string Question { get; set; }
        = string.Empty;


    // ==========================================
    // User Language
    // Example:
    // en-US
    // km-KH
    // ==========================================
    public string Language { get; set; }
        = "en-US";



    // ==========================================
    // Optional Lesson Reference
    // Used when asking about specific lesson
    // ==========================================
    public int? LessonId { get; set; }



    // ==========================================
    // Student Personalization
    // Used for:
    // - GPA
    // - Attendance
    // - Subjects
    // - Recommendations
    // ==========================================
    public int StudentId { get; set; }



    // ==========================================
    // Conversation Context (Future)
    // ==========================================
    public int? ConversationId { get; set; }

}