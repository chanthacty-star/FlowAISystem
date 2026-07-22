using FlowAISystem.Domain.Common;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Domain.Entities;

public class Announcement : BaseEntity
{
    public string Title { get; set; }
        = string.Empty;


    public string Content { get; set; }
        = string.Empty;



    public DateTime PublishDate { get; set; }


    public DateTime? ExpireDate { get; set; }



    // Who can see this announcement

    public AnnouncementAudience Audience { get; set; }



    // Created By User

    public int CreatedByUserId { get; set; }


    public User? CreatedByUser { get; set; }

}