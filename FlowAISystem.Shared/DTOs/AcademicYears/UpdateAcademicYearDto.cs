using System.ComponentModel.DataAnnotations;

namespace FlowAISystem.Shared.DTOs.AcademicYears;

public class UpdateAcademicYearDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    public bool IsCurrent { get; set; }
}