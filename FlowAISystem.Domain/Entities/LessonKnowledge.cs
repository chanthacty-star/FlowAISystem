using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class LessonKnowledge : BaseEntity
{
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
    // Teacher Owner
    // ==================================================

    public int TeacherId { get; set; }


    public User? Teacher { get; set; }



    // ==================================================
    // Subject Relation
    // ==================================================

    public int? CourseOfferingId { get; set; }


    public CourseOffering? CourseOffering { get; set; }



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