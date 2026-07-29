namespace FlowAISystem.Domain.Entities.AI;

public class AIConversation
{
    public int Id { get; set; }


    // Owner
    public int UserId { get; set; }


    public string Title { get; set; }
        = "New Conversation";


    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;


    public DateTime UpdatedAt { get; set; }
        = DateTime.UtcNow;



    // Navigation

    public ICollection<AIMessage> Messages { get; set; }
        = new List<AIMessage>();
}