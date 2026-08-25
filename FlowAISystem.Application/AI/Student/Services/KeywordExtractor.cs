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
            .ToLowerInvariant()
            .Trim();

        // ==================================================
        // English Stop Words
        // ==================================================

        var englishIgnored = new HashSet<string>
        {
            "explain",
            "what",
            "what's",
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
            "could",
            "would",
            "you",
            "your",
            "i",
            "want",
            "to",
            "learn",
            "of",
            "for",
            "with",
            "and",
            "or",
            "in",
            "on",
            "from",
            "this",
            "that",
            "it",
            "its"
        };

        // ==================================================
        // Khmer Stop Words
        // ==================================================

        var khmerIgnored = new HashSet<string>
        {
            "សូម",
            "ខ្ញុំ",
            "អ្នក",
            "អំពី",
            "នៃ",
            "ជា",
            "គឺ",
            "ហើយ",
            "ដែល",
            "នេះ",
            "នោះ",
            "ផង",
            "បាន",
            "មួយ",
            "មក",
            "ទៅ",
            "ឲ្យ",
            "អោយ",
            "ពន្យល់",
            "ប្រាប់",
            "បង្ហាញ",
            "សួរ",
            "រៀន",
            "ចង់",
            "តើ",
            "អ្វី",
            "យ៉ាង",
            "ដូចម្តេច"
        };

        // ==================================================
        // Normalize punctuation
        // ==================================================

        var normalized = message
            .Replace("?", " ")
            .Replace("!", " ")
            .Replace(",", " ")
            .Replace(".", " ")
            .Replace(":", " ")
            .Replace(";", " ")
            .Replace("(", " ")
            .Replace(")", " ")
            .Replace("[", " ")
            .Replace("]", " ")
            .Replace("{", " ")
            .Replace("}", " ")
            .Replace("\n", " ")
            .Replace("\r", " ")
            .Replace("\t", " ");

        var words = normalized
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        var keywords = new List<string>();

        // ==================================================
        // Single-word keywords
        // ==================================================

        foreach (var word in words)
        {
            if (englishIgnored.Contains(word))
                continue;

            if (khmerIgnored.Contains(word))
                continue;

            // English
            if (word.All(c => c < 128))
            {
                if (word.Length > 2)
                {
                    keywords.Add(word);
                }

                continue;
            }

            // Khmer
            keywords.Add(word);
        }

        // ==================================================
        // Common multi-word AI concepts
        // ==================================================

        for (int i = 0; i < words.Count - 1; i++)
        {
            var first = words[i];
            var second = words[i + 1];

            if (englishIgnored.Contains(first) ||
                englishIgnored.Contains(second))
            {
                continue;
            }

            if (first.Length <= 1 || second.Length <= 1)
            {
                continue;
            }

            var phrase =
                $"{first} {second}";

            keywords.Add(phrase);
        }

        // ==================================================
        // Return distinct keywords
        // ==================================================

        return keywords
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}