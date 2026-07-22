using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Announcements;

public class AnnouncementListItemDto
{
    public int Id { get; set; }


    public string Title { get; set; }
        = string.Empty;


    public AnnouncementAudience Audience { get; set; }


    public string CreatedByName { get; set; }
        = string.Empty;


    public DateTime PublishDate { get; set; }


    public DateTime? ExpireDate { get; set; }
}