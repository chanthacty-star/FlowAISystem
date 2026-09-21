using System.Text.RegularExpressions;
using FlowAISystem.Application.AI.Student.Conversation.Enums;
using FlowAISystem.Application.AI.Student.Conversation.Interfaces;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class ConversationActionDetector : IConversationActionDetector
{
    // Specific, low-ambiguity phrases — checked first.
    private static readonly (ConversationAction Action, string[] Phrases)[] SpecificPhrases =
    {
        (ConversationAction.ExplainMore, new[]
        {
            "explain more", "explain further", "more detail", "more details",
            "explain again", "tell me more",
            "ពន្យល់បន្ថែម", "ពន្យល់ម្ដងទៀត", "ពន្យល់ម្តងទៀត", "ប្រាប់បន្ថែម"
        }),
        (ConversationAction.ShowExample, new[]
        {
            "show example", "give example", "give me an example", "practical example",
            "បង្ហាញឧទាហរណ៍", "ឧទាហរណ៍ជាក់ស្តែង"
        }),
        (ConversationAction.CreateQuiz, new[]
        {
            "create quiz", "make a quiz", "give me a quiz", "quiz me", "test me",
            "practice questions", "practice question",
            "ធ្វើតេស្ត", "សំណួរអនុវត្ត", "ប្រឡង"
        }),
        (ConversationAction.Translate, new[]
        {
            "translate this", "translate it", "translate to khmer", "translate into khmer",
            "បកប្រែជាខ្មែរ", "បកប្រែទៅខ្មែរ"
        }),
        (ConversationAction.Compare, new[]
        {
            "compare them", "difference between", "comparison",
            "ប្រៀបធៀប", "ភាពខុសគ្នា"
        }),
        (ConversationAction.Summarize, new[]
        {
            "summarize this", "short summary", "summarise",
            "សង្ខេបមេរៀន", "សង្ខេបអត្ថបទ"
        }),
        (ConversationAction.Continue, new[]
        {
            "keep going", "go on",
            "បន្តទៅ", "បន្តទៀត"
        }),
    };

    // Generic single-word fallbacks — checked only if no specific phrase matched.
    private static readonly (ConversationAction Action, string[] Words)[] FallbackWords =
    {
        (ConversationAction.CreateQuiz, new[] { "quiz", "សំណួរ" }),
        (ConversationAction.ShowExample, new[] { "example", "ឧទាហរណ៍" }),
        (ConversationAction.Translate, new[] { "translate", "បកប្រែ" }),
        (ConversationAction.Compare, new[] { "compare", "ប្រៀបធៀប" }),
        (ConversationAction.Summarize, new[] { "summary", "summarize", "សង្ខេប" }),
        (ConversationAction.Continue, new[] { "continue", "បន្ត" }),
        // "next" deliberately excluded — it now belongs to Recommendation
        // ("next lesson"), not "continue this topic."
    };

    public ConversationAction Detect(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return ConversationAction.None;

        var normalized = Normalize(message);

        foreach (var (action, phrases) in SpecificPhrases)
        {
            if (phrases.Any(p => normalized.Contains(p, StringComparison.OrdinalIgnoreCase)))
                return action;
        }

        foreach (var (action, words) in FallbackWords)
        {
            if (words.Any(w => ContainsWord(normalized, w)))
                return action;
        }

        return ConversationAction.None;
    }

    private bool ContainsWord(string text, string word)
    {
        // Word-boundary match for ASCII (English); Khmer script has no
        // reliable \b support in .NET regex, so it falls back to substring.
        if (word.All(c => c < 128))
        {
            return Regex.IsMatch(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase);
        }

        return text.Contains(word, StringComparison.OrdinalIgnoreCase);
    }

    private string Normalize(string message)
        => message.Trim().ToLowerInvariant().Replace("\r", " ").Replace("\n", " ");
}