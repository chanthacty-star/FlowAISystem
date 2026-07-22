namespace FlowAISystem.Shared.DTOs.Teachers;

public class TeacherProfileDto
{
    public int Id { get; set; }


    // Personal Information

    public string TeacherName { get; set; } = string.Empty;


    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;


    public string PhoneNumber { get; set; } = string.Empty;


    public string Gender { get; set; } = string.Empty;


    public DateOnly DateOfBirth { get; set; }


    public DateOnly HireDate { get; set; }



    // Department

    public string DepartmentName { get; set; } = string.Empty;



    // Account

    public int? UserId { get; set; }


    public string Username { get; set; } = string.Empty;


    public bool AccountActive { get; set; }



    // Image

    public string? ProfileImage { get; set; }
}