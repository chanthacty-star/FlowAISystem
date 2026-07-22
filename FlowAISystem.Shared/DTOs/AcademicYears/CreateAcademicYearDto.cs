using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.AcademicYears;

public class CreateAcademicYearDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    public bool IsCurrent { get; set; }
}