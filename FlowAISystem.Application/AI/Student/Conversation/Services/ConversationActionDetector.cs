using FlowAISystem.Application.AI.Student.Conversation.Enums;
using FlowAISystem.Application.AI.Student.Conversation.Interfaces;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class ConversationActionDetector
    : IConversationActionDetector
{
    public ConversationAction Detect(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return ConversationAction.None;

        message = Normalize(message);


        // ==========================================
        // 1. Explain More
        // ==========================================

        if (
            message.Contains("explain more") ||
            message.Contains("explain further") ||
            message.Contains("more detail") ||
            message.Contains("more details") ||
            message.Contains("explain again") ||
            message.Contains("tell me more") ||

            // Khmer
            message.Contains("ពន្យល់បន្ថែម") ||
            message.Contains("ពន្យល់ម្ដងទៀត") ||
            message.Contains("ពន្យល់ម្តងទៀត") ||
            message.Contains("ប្រាប់បន្ថែម")
        )
        {
            return ConversationAction.ExplainMore;
        }


        // ==========================================
        // 2. Show Example
        // ==========================================

        if (
            message.Contains("show example") ||
            message.Contains("give example") ||
            message.Contains("give me an example") ||
            message.Contains("practical example") ||
            message.Contains("example") ||

            // Khmer
            message.Contains("ឧទាហរណ៍") ||
            message.Contains("បង្ហាញឧទាហរណ៍") ||
            message.Contains("ឧទាហរណ៍ជាក់ស្តែង")
        )
        {
            return ConversationAction.ShowExample;
        }


        // ==========================================
        // 3. Create Quiz
        // ==========================================

        if (
            message.Contains("create quiz") ||
            message.Contains("make a quiz") ||
            message.Contains("give me a quiz") ||
            message.Contains("quiz me") ||
            message.Contains("test me") ||
            message.Contains("practice questions") ||
            message.Contains("practice question") ||
            message.Contains("quiz") ||

            // Khmer
            message.Contains("ធ្វើតេស្ត") ||
            message.Contains("សំណួរអនុវត្ត") ||
            message.Contains("សំណួរ") ||
            message.Contains("ប្រឡង")
        )
        {
            return ConversationAction.CreateQuiz;
        }


        // ==========================================
        // 4. Translate
        // ==========================================

        if (
            message.Contains("translate") ||
            message.Contains("translate this") ||
            message.Contains("translate it") ||
            message.Contains("translate to khmer") ||
            message.Contains("translate into khmer") ||

            // Khmer
            message.Contains("បកប្រែ") ||
            message.Contains("បកប្រែជាខ្មែរ") ||
            message.Contains("បកប្រែទៅខ្មែរ")
        )
        {
            return ConversationAction.Translate;
        }


        // ==========================================
        // 5. Continue
        // ==========================================

        if (
            message == "continue" ||
            message.Contains("continue") ||
            message.Contains("keep going") ||
            message.Contains("go on") ||
            message.Contains("next") ||

            // Khmer
            message == "បន្ត" ||
            message.Contains("បន្តទៅ") ||
            message.Contains("បន្តទៀត")
        )
        {
            return ConversationAction.Continue;
        }


        // ==========================================
        // 6. Compare
        // ==========================================

        if (
            message.Contains("compare") ||
            message.Contains("comparison") ||
            message.Contains("compare them") ||
            message.Contains("difference between") ||

            // Khmer
            message.Contains("ប្រៀបធៀប") ||
            message.Contains("ភាពខុសគ្នា")
        )
        {
            return ConversationAction.Compare;
        }


        // ==========================================
        // 7. Summarize
        // ==========================================

        if (
            message.Contains("summary") ||
            message.Contains("summarize") ||
            message.Contains("summarise") ||
            message.Contains("summarize this") ||
            message.Contains("short summary") ||

            // Khmer
            message.Contains("សង្ខេប") ||
            message.Contains("សង្ខេបមេរៀន") ||
            message.Contains("សង្ខេបអត្ថបទ")
        )
        {
            return ConversationAction.Summarize;
        }


        // ==========================================
        // 8. No Conversation Action
        // ==========================================

        return ConversationAction.None;
    }


    // ==========================================
    // Normalize Message
    // ==========================================

    private string Normalize(string message)
    {
        return message
            .Trim()
            .ToLowerInvariant()
            .Replace("\r", " ")
            .Replace("\n", " ");
    }
}

