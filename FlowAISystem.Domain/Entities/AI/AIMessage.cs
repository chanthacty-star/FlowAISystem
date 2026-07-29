namespace FlowAISystem.Domain.Entities.AI;


public class AIMessage
{

    public int Id { get; set; }



    public int ConversationId { get; set; }



    public string Role { get; set; }
        = string.Empty;


    /*
       User
       Assistant
    */


    public string Content { get; set; }
        = string.Empty;



    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;



    // Navigation

    public AIConversation Conversation { get; set; }
        = null!;
}