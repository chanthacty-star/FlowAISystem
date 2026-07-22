using FlowAISystem.Domain.Common;
using FlowAISystem.Domain.ValueObjects;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Domain.Entities;

public class Teacher : BaseEntity
{
    public string TeacherName { get; set; } = string.Empty;


    public PersonName Name { get; set; }
        = new("", "");


    public string Email { get; set; } = string.Empty;


    public string PhoneNumber { get; set; } = string.Empty;


    public Gender Gender { get; set; }


    public DateOnly DateOfBirth { get; set; }


    public DateOnly HireDate { get; set; }



    // User Account Relationship

    public int? UserId { get; set; }

    public User? User { get; set; }



    // Department Relationship

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }



    // Profile Image

    public string? ProfileImage { get; set; }



    // Teacher 1 ---> Many CourseOfferings

    public ICollection<CourseOffering> CourseOfferings { get; set; }
        = new List<CourseOffering>();
}