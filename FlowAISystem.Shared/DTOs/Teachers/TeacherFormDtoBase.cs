using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Teachers;


public class TeacherFormDtoBase
{

    [Required]
    [MaxLength(100)]
    public string TeacherName { get; set; }
        = string.Empty;


    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
        = string.Empty;


    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
        = string.Empty;


    [Required]
    [EmailAddress]
    public string Email { get; set; }
        = string.Empty;


    public string PhoneNumber { get; set; }
        = string.Empty;


    public Gender Gender { get; set; }


    public DateOnly DateOfBirth { get; set; }


    public DateOnly HireDate { get; set; }


    [Required]
    public int DepartmentId { get; set; }

}