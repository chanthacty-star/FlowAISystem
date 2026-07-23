namespace FlowAISystem.Shared.DTOs.Students;

public class StudentAnnouncementSummaryDto
{
    public int Id { get; set; }

    public string Title { get; set; }
        = string.Empty;

    public DateTime PublishDate { get; set; }
}