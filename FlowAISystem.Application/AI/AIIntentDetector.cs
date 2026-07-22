using FlowAISystem.Application.AI.Interfaces;

namespace FlowAISystem.Application.AI.Services;

public class AIIntentDetector : IAIIntentDetector
{
    public AIIntent Detect(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return AIIntent.General;


        message = message.ToLower();


        if (message.Contains("hello") ||
            message.Contains("hi") ||
            message.Contains("hey"))
        {
            return AIIntent.Greeting;
        }


        if (message.Contains("student"))
        {
            return AIIntent.Student;
        }


        if (message.Contains("teacher") ||
            message.Contains("teachers") ||
            message.Contains("department") ||
            message.Contains("departments") ||
            message.Contains("subject") ||
            message.Contains("subjects") ||
            message.Contains("user") ||
            message.Contains("users"))
        {
            return AIIntent.Dashboard;
        }


        if (message.Contains("dashboard") ||
            message.Contains("summary"))
        {
            return AIIntent.Dashboard;
        }


        if (message.Contains("knowledge") ||
            message.Contains("ai") ||
            message.Contains("what is") ||
            message.Contains("explain") ||
            message.Contains("tell me") ||
            message.Contains("information") ||
            message.Contains("about"))
        {
            return AIIntent.AIKnowledge;
        }


        if (message.Contains("report") ||
            message.Contains("pdf") ||
            message.Contains("excel"))
        {
            return AIIntent.Report;
        }


        return AIIntent.General;
    }
}