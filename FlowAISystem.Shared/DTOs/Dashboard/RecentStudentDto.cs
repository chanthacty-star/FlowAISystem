namespace FlowAISystem.Shared.DTOs.Dashboard;

public class RecentStudentDto
{
    public string FullName { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}