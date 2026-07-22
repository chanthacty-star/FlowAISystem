using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Attendances;

public class CreateAttendanceDto
{
    [Required]
    public int EnrollmentId { get; set; }

    [Required]
    public DateOnly AttendanceDate { get; set; }

    public AttendanceStatus Status { get; set; }
        = AttendanceStatus.Present;

    [StringLength(200)]
    public string? Remark { get; set; }
}