using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;

public class StudentIntentDetector : IStudentIntentDetector
{
    public StudentIntent Detect(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return StudentIntent.Unknown;
        }


        message = message.ToLower().Trim();



        // ==========================================
        // Greeting
        // ==========================================

        if (message.Contains("hello") ||
            message.Contains("hi") ||
            message.Contains("hey") ||
            message.Contains("good morning") ||
            message.Contains("good afternoon"))
        {
            return StudentIntent.Greeting;
        }



        // ==========================================
        // Quiz Request
        // ==========================================

        if (message.Contains("quiz") ||
            message.Contains("test me") ||
            message.Contains("practice question") ||
            message.Contains("give me questions"))
        {
            return StudentIntent.Quiz;
        }



        // ==========================================
        // Recommendation
        // ==========================================

        if (message.Contains("recommend") ||
            message.Contains("what should i study") ||
            message.Contains("what should i learn") ||
            message.Contains("next topic"))
        {
            return StudentIntent.Recommendation;
        }



        // ==========================================
        // Academic Information
        // ==========================================

        if (message.Contains("gpa") ||
            message.Contains("grade") ||
            message.Contains("score") ||
            message.Contains("attendance") ||
            message.Contains("subject") ||
            message.Contains("schedule") ||
            message.Contains("my class") ||
            message.Contains("my course"))
        {
            return StudentIntent.AcademicInformation;
        }



        // ==========================================
        // Learning / Explanation
        // ==========================================

        if (message.Contains("explain") ||
            message.Contains("what is") ||
            message.Contains("what are") ||
            message.Contains("tell me about") ||
            message.Contains("how does") ||
            message.Contains("define") ||
            message.Contains("meaning of"))
        {
            return StudentIntent.Learn;
        }



        // ==========================================
        // Conversation Follow-up
        // ==========================================

        if (message.Contains("example") ||
            message.Contains("again") ||
            message.Contains("compare") ||
            message.Contains("more detail") ||
            message.Contains("continue"))
        {
            return StudentIntent.Conversation;
        }



        // ==========================================
        // Default
        // ==========================================

        return StudentIntent.General;
    }
}