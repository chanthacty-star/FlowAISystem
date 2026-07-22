namespace FlowAISystem.Application.AI.Utilities;

public static class KeywordExtractor
{
    private static readonly HashSet<string> StopWords =
    [
        "what",
        "is",
        "are",
        "was",
        "were",
        "am",
        "explain",
        "tell",
        "me",
        "about",
        "the",
        "a",
        "an",
        "please",
        "can",
        "could",
        "would",
        "you",
        "how",
        "does",
        "do",
        "did",
        "of",
        "to",
        "for",
        "in",
        "on",
        "at",
        "and",
        "or",
        "with",
        "my"
    ];

    public static List<string> Extract(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return [];

        return message
            .ToLower()
            .Replace("?", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("!", "")
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !StopWords.Contains(word))
            .Distinct()
            .ToList();
    }
}