namespace FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

public class LessonKnowledgeDto
{
    // ==================================================
    // Identity
    // ==================================================

    public int Id { get; set; }

    // ==================================================
    // Lesson Information
    // ==================================================

    public string Title { get; set; }
        = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public string Content { get; set; }
        = string.Empty;

    // ==================================================
    // AI Knowledge
    // ==================================================

    public string Keywords { get; set; }
        = string.Empty;

    public string Category { get; set; }
        = string.Empty;

    // ==================================================
    // Teacher Information
    // ==================================================

    public int TeacherId { get; set; }

    public string TeacherName { get; set; }
        = string.Empty;

    // ==================================================
    // Course Information
    // ==================================================

    public int CourseOfferingId { get; set; }

    public string CourseName { get; set; }
        = string.Empty;

    // ==================================================
    // Learning Material
    // ==================================================

    public string? ReferenceUrl { get; set; }

    public string? AttachmentPath { get; set; }

    // ==================================================
    // Status
    // ==================================================

    public bool IsActive { get; set; }

    // ==================================================
    // Audit
    // ==================================================

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}