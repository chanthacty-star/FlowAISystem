using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Announcements;

public class AnnouncementSearchDto
{
    public string? SearchTerm { get; set; }


    public AnnouncementAudience? Audience { get; set; }


    public DateTime? FromDate { get; set; }


    public DateTime? ToDate { get; set; }
}