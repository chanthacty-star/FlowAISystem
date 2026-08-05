namespace FlowAISystem.Shared.DTOs.AI;


public class AIMessageDto
{

    public int Id { get; set; }


    public string Role { get; set; }
        = string.Empty;


    public string Content { get; set; }
        = string.Empty;

    //public string StudentName { get; set; } = string.Empty; 

    public DateTime CreatedAt { get; set; }

}