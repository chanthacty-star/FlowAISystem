using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class LessonKnowledge : BaseEntity
{

    // Lesson information

    public string Title { get; set; }
        = string.Empty;



    public string Content { get; set; }
        = string.Empty;



    // Keywords for AI searching

    public string Keywords { get; set; }
        = string.Empty;



    // Teacher owner

    public int TeacherId { get; set; }


    public User? Teacher { get; set; }



    // Subject relation

    public int? CourseOfferingId { get; set; }


    public CourseOffering? CourseOffering { get; set; }



    // Status

    public bool IsActive { get; set; }
        = true;

}