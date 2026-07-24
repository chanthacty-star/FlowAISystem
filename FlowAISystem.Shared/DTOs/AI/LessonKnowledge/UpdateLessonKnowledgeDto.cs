namespace FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

public class UpdateLessonKnowledgeDto
{
    // ==================================================
    // Identity
    // ==================================================

    public int Id { get; set; }

    // ==================================================
    // Basic Information
    // ==================================================

    public string Title { get; set; }
        = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public string Content { get; set; }
        = string.Empty;

    // ==================================================
    // AI Search
    // ==================================================

    public string Keywords { get; set; }
        = string.Empty;

    public string Category { get; set; }
        = string.Empty;

    // ==================================================
    // Academic Information
    // ==================================================

    public int TeacherId { get; set; }

    //public int CourseOfferingId { get; set; }// berlong to db
    public int? CourseOfferingId { get; set; }// berlong to null


    // ==================================================
    // Learning Material
    // ==================================================

    public string? ReferenceUrl { get; set; }

    public string? AttachmentPath { get; set; }

    // ==================================================
    // Status
    // ==================================================

    public bool IsActive { get; set; }
}