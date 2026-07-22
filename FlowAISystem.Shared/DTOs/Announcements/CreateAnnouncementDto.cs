using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Announcements;

public class CreateAnnouncementDto
{

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
        = string.Empty;



    [Required]
    public string Content { get; set; }
        = string.Empty;



    [Required]
    public DateTime PublishDate { get; set; }
        = DateTime.Now;



    public DateTime? ExpireDate { get; set; }



    public AnnouncementAudience Audience { get; set; }
        = AnnouncementAudience.All;



    public int CreatedByUserId { get; set; }

}