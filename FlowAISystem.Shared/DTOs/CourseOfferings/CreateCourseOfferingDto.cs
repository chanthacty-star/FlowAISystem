using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.CourseOfferings;

public class CreateCourseOfferingDto
{

    [Required]
    [StringLength(50)]
    public string ClassName { get; set; } = string.Empty;



    [Range(1, 200)]
    public int Capacity { get; set; }



    public string? Room { get; set; }



    [Required]
    public int SubjectId { get; set; }



    [Required]
    public int TeacherId { get; set; }



    [Required]
    public int SemesterId { get; set; }
}