using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Enrollments;

public class UpdateEnrollmentDto
{
    public int Id { get; set; }



    [Required]
    public int StudentId { get; set; }



    [Required]
    public int CourseOfferingId { get; set; }



    public EnrollmentStatus Status { get; set; }



    [Required]
    public DateOnly EnrollmentDate { get; set; }
}