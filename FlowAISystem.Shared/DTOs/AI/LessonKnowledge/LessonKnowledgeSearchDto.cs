using FlowAISystem.Shared.Enums;
namespace FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

public class LessonKnowledgeSearchDto
{
    // ==================================================
    // Text Search
    // ==================================================

    public string? SearchTerm { get; set; }
    public LessonDifficulty? Difficulty { get; set; }
    //public int Order { get; set; }

    // ==================================================
    // Filter
    // ==================================================

    public string? Category { get; set; }


    public int? TeacherId { get; set; }


    public int? CourseOfferingId { get; set; }


    //public bool? IsPublished { get; set; }
    public bool? IsActive { get; set; }


    // ==================================================
    // Pagination
    // ==================================================

    public int PageNumber { get; set; } = 1;


    public int PageSize { get; set; } = 10;
}