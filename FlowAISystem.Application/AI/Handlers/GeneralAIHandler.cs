using FlowAISystem.Application.AI.Interfaces;

namespace FlowAISystem.Application.AI.Handlers;

public class GeneralAIHandler : IGeneralAIHandler
{
    public async Task<string> HandleAsync(string message)
    {
        await Task.CompletedTask;

        message = message.Trim().ToLower();

        // ==========================================
        // Greetings
        // ==========================================

        if (message.Contains("hello") ||
            message.Contains("hi") ||
            message.Contains("hey"))
        {
            return
                "Hello! I'm FlowAI Assistant. How can I help you today?";
        }

        // ==========================================
        // Morning
        // ==========================================

        if (message.Contains("good morning"))
        {
            return
                "Good morning! I hope you have a productive day.";
        }

        // ==========================================
        // Afternoon
        // ==========================================

        if (message.Contains("good afternoon"))
        {
            return
                "Good afternoon! What can I help you with?";
        }

        // ==========================================
        // Evening
        // ==========================================

        if (message.Contains("good evening"))
        {
            return
                "Good evening! How may I assist you?";
        }

        // ==========================================
        // AI Name
        // ==========================================

        if (message.Contains("your name") ||
            message.Contains("who are you"))
        {
            return
                "I am FlowAI Assistant, your intelligent assistant for FlowAISystem.";
        }

        // ==========================================
        // Thank You
        // ==========================================

        if (message.Contains("thank"))
        {
            return
                "You're welcome! I'm always happy to help.";
        }

        // ==========================================
        // Goodbye
        // ==========================================

        if (message.Contains("bye") ||
            message.Contains("goodbye"))
        {
            return
                "Goodbye! Have a wonderful day.";
        }

        // ==========================================
        // Unknown
        // ==========================================

        return
            "I'm not sure how to answer that yet. Please try asking about students, dashboard information, or AI knowledge.";
    }
}