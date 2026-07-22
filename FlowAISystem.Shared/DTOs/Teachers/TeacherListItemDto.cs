namespace FlowAISystem.Shared.DTOs.Teachers;

public class TeacherListItemDto
{
    public int Id { get; set; }


    public string TeacherName { get; set; }
        = string.Empty;


    public string FirstName { get; set; }
        = string.Empty;


    public string LastName { get; set; }
        = string.Empty;

    public string FullName =>
        $"{FirstName} {LastName}";

    public string Email { get; set; }
        = string.Empty;


    public string PhoneNumber { get; set; }
        = string.Empty;


    public string DepartmentName { get; set; }
        = string.Empty;
}