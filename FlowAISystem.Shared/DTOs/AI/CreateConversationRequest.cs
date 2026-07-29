namespace FlowAISystem.Shared.DTOs.AI;


public class CreateConversationRequest
{

    public int UserId { get; set; }


    public string Title { get; set; }
        = "New Conversation";

}