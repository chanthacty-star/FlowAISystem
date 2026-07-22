using FlowAISystem.Domain.Common;
using FlowAISystem.Domain.ValueObjects;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Domain.Entities;

public class Student : BaseEntity
{
    public string StudentNumber { get; set; } = string.Empty;

    public PersonName Name { get; set; } = new("", "");

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    // User account

    public int? UserId { get; set; }

    public User? User { get; set; }
    //tudene images
    public string? ProfileImage { get; set; }

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
        = new List<Enrollment>();

    public ICollection<Feedback> Feedbacks { get; set; }
        = new List<Feedback>();

    public ICollection<Prediction> Predictions { get; set; }
        = new List<Prediction>();
}