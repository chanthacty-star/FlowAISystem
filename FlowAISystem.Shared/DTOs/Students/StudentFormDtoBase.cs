using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Students;

public abstract class StudentFormDtoBase
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(
        50,
        MinimumLength = 2,
        ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(
        50,
        MinimumLength = 2,
        ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string LastName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Student number is required.")]
    [StringLength(
        20,
        MinimumLength = 3,
        ErrorMessage = "Student number must be between 3 and 20 characters.")]
    public string StudentNumber { get; set; } = string.Empty;


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;


    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; }


    public DateOnly DateOfBirth { get; set; }


    public DateOnly EnrollmentDate { get; set; }
        = DateOnly.FromDateTime(DateTime.Today);


    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Please select a department.")]
    public int DepartmentId { get; set; }
}