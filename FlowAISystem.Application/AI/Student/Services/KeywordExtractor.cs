using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;

public class KeywordExtractor : IKeywordExtractor
{
    public IEnumerable<string> Extract(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return Enumerable.Empty<string>();
        }


        message = message
            .ToLower()
            .Trim();



        var keywords = new List<string>();


        // ==========================================
        // Remove common words
        // ==========================================

        var ignoredWords = new HashSet<string>
        {
            "explain",
            "what",
            "is",
            "are",
            "the",
            "a",
            "an",
            "about",
            "tell",
            "me",
            "show",
            "give",
            "please",
            "my",
            "how",
            "does",
            "do",
            "can",
            "you",
            "i",
            "want",
            "to",
            "learn"
        };



        var words = message
            .Split(
                new[]
                {
                    ' ',
                    ',',
                    '.',
                    '?',
                    '!'
                },
                StringSplitOptions.RemoveEmptyEntries);



        foreach (var word in words)
        {
            if (!ignoredWords.Contains(word)
                && word.Length > 2)
            {
                keywords.Add(word);
            }
        }



        return keywords
            .Distinct()
            .ToList();
    }
}