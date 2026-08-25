using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

public class CreateLessonKnowledgeDto
{
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
    public LessonDifficulty Difficulty { get; set; }
    = LessonDifficulty.Beginner;
    public string? ActivityType { get; set; }
    public int Order { get; set; } 

    // ==================================================
    // Academic Information
    // ==================================================

    public int TeacherId { get; set; }

    //public int CourseOfferingId { get; set; } // belong to database only
    public int? CourseOfferingId { get; set; } // can be null here 

    // ==================================================
    // Learning Material
    // ==================================================

    public string? ReferenceUrl { get; set; }

    public string? AttachmentPath { get; set; }

    // ==================================================
    // Status
    // ==================================================

    public bool IsActive { get; set; }
    = true;
}