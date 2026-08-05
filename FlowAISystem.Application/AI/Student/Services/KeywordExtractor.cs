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
        // English Stop Words
        // ==========================================

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
            "on"
        };

        // ==========================================
        // Khmer Stop Words
        // ==========================================

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
            "ដូចម្តេច",
            "ដូចម្តេច?"
        };

        // ==========================================
        // Split
        // ==========================================

        var words = message.Split(
            new[]
            {
                ' ',
                ',',
                '.',
                '?',
                '!',
                ':',
                ';',
                '\n',
                '\r',
                '\t'
            },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            var token = word.Trim();

            if (string.IsNullOrWhiteSpace(token))
                continue;

            if (englishIgnored.Contains(token))
                continue;

            if (khmerIgnored.Contains(token))
                continue;

            // English keyword
            if (token.All(c => c < 128))
            {
                if (token.Length > 2)
                {
                    keywords.Add(token);
                }

                continue;
            }

            // Khmer keyword
            keywords.Add(token);
        }

        return keywords
            .Distinct()
            .ToList();
    }
}