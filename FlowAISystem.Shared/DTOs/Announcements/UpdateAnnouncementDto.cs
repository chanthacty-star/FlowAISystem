using System.ComponentModel.DataAnnotations;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Shared.DTOs.Announcements;

public class UpdateAnnouncementDto
{

    public int Id { get; set; }



    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
        = string.Empty;



    [Required]
    public string Content { get; set; }
        = string.Empty;



    public DateTime PublishDate { get; set; }



    public DateTime? ExpireDate { get; set; }



    public AnnouncementAudience Audience { get; set; }

}