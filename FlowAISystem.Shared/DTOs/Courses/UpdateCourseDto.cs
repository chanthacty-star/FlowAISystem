using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.Courses;

public class UpdateCourseDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Credits { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int TeacherId { get; set; }

    [Required]
    public int SemesterId { get; set; }
}