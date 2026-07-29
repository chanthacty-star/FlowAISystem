using FlowAISystem.Application.AI.Interfaces;


namespace FlowAISystem.Application.AI.Services;


public class ConversationTitleService
    : IConversationTitleService
{

    public string GenerateTitle(string question)
    {

        if (string.IsNullOrWhiteSpace(question))
            return "New Conversation";


        question = question.Trim();



        if (question.Length <= 40)
            return question;



        return question.Substring(0, 40) + "...";

    }

}