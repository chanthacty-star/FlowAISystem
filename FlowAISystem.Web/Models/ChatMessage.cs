namespace FlowAISystem.Web.Models;

using FlowAISystem.Shared.DTOs.AI;
public class ChatMessage
{

    public string Text { get; set; } = string.Empty;



    public bool IsUser { get; set; }

    public string SenderName { get; set; }
    = string.Empty;// can be many user 

    public DateTime Time { get; set; }
        = DateTime.Now;



    // AI Response Data
    public StudentAIResponseDto? Response { get; set; }



    // Typing animation control
    // true = animate
    // false = display immediately
    public bool EnableTyping { get; set; }



    // Database message id (optional)
    public int? MessageId { get; set; }


}