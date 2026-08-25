using FlowAISystem.Shared.Enums;
namespace FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

public class LessonKnowledgeListItemDto
{
    // ==================================================
    // Identity
    // ==================================================

    public int Id { get; set; }

    // ==================================================
    // Lesson
    // ==================================================

    public string Title { get; set; }
        = string.Empty;

    public string Category { get; set; }
        = string.Empty;
    public LessonDifficulty Difficulty { get; set; }
    = LessonDifficulty.Beginner;
    public string? ActivityType { get; set; }
    public int Order { get; set; }

    // ==================================================
    // Teacher
    // ==================================================

    public int TeacherId { get; set; }

    public string TeacherName { get; set; }
        = string.Empty;

    // ==================================================
    // Course
    // ==================================================

    public int? CourseOfferingId { get; set; }

    public string CourseName { get; set; }
        = string.Empty;

    // ==================================================
    // Status
    // ==================================================

    public bool IsActive { get; set; }

    // ==================================================
    // Statistics
    // ==================================================

    public int KeywordCount { get; set; }

    public int ContentLength { get; set; }

    // ==================================================
    // Audit
    // ==================================================

    public DateTime CreatedAt { get; set; }
}