using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.Enums;

namespace FlowAISystem.Application.AI.Student.Services;

/// <summary>
/// Formats raw student AI lesson content into structured,
/// multi-language and difficulty-aware responses.
/// </summary>
public class StudentAIResponseFormatter : IStudentAIResponseFormatter
{
    /// <summary>
    /// Converts lesson content into a structured response based on
    /// the student's selected language and lesson difficulty.
    /// </summary>
    /// <param name="title">The title of the lesson.</param>
    /// <param name="content">The raw generated or retrieved AI content.</param>
    /// <param name="difficulty">Target difficulty level (Beginner, Intermediate, Advanced).</param>
    /// <param name="language">Language code (e.g., "km-KH" for Khmer).</param>
    /// <returns>A formatted string with difficulty context, localized headers, and custom summaries.</returns>
    public string Format(
        string title,
        string content,
        LessonDifficulty difficulty,
        string language)
    {
        // Guard clause for empty content
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        bool isKhmer = language == "km-KH";
        var result = new List<string>();

        // ==========================================
        // 1. TITLE
        // ==========================================
        result.Add($"""
📘 {title}
""");

        // ==========================================
        // 2. DIFFICULTY
        // ==========================================
        result.Add($"""
🎯 {(isKhmer ? "កម្រិតមេរៀន" : "Difficulty")}: {GetDifficultyName(difficulty, isKhmer)}
""");

        // ==========================================
        // 3. EXPLANATION
        // ==========================================
        result.Add($"""
🔎 {(isKhmer ? "ការពន្យល់" : "Explanation")}
{GetDifficultyExplanation(ExtractExplanation(content), difficulty, isKhmer)}
""");

        // ==========================================
        // 4. STEPS / HOW IT WORKS
        // ==========================================
        var steps = ExtractSection(content, "Steps");
        if (!string.IsNullOrWhiteSpace(steps))
        {
            result.Add($"""
⚙️ {(isKhmer ? "របៀបដំណើរការ" : "How It Works")}
{steps}
""");
        }

        // ==========================================
        // 5. EXAMPLE
        // ==========================================
        var example = ExtractSection(content, "Example");
        if (!string.IsNullOrWhiteSpace(example))
        {
            result.Add($"""
💡 {(isKhmer ? "ឧទាហរណ៍" : "Example")}
{example}
""");
        }

        // ==========================================
        // 6. TIME COMPLEXITY
        // ==========================================
        var complexity = ExtractSection(content, "Time Complexity");
        if (!string.IsNullOrWhiteSpace(complexity))
        {
            result.Add($"""
⏱ {(isKhmer ? "ភាពស្មុគស្មាញពេលវេលា" : "Time Complexity")}
{complexity}
""");
        }

        // ==========================================
        // 7. ADVANCED INFORMATION
        // ==========================================
        if (difficulty == LessonDifficulty.Advanced)
        {
            var complexitySection = ExtractSection(content, "Complexity");

            if (!string.IsNullOrWhiteSpace(complexitySection) &&
                !complexitySection.Equals(complexity, StringComparison.OrdinalIgnoreCase))
            {
                result.Add($"""
🧠 {(isKhmer ? "ព័ត៌មានបន្ថែម" : "Advanced Details")}
{complexitySection}
""");
            }
        }

        // ==========================================
        // 8. SUMMARY
        // ==========================================
        result.Add($"""
✅ {(isKhmer ? "សង្ខេប" : "Summary")}
{BuildSummary(title, difficulty, isKhmer)}
""");

        return string.Join(
            Environment.NewLine + Environment.NewLine,
            result);
    }

    /// <summary>
    /// Resolves the localized display name for a given difficulty level.
    /// </summary>
    private string GetDifficultyName(LessonDifficulty difficulty, bool isKhmer)
    {
        if (isKhmer)
        {
            return difficulty switch
            {
                LessonDifficulty.Beginner => "កម្រិតដំបូង",
                LessonDifficulty.Intermediate => "កម្រិតមធ្យម",
                LessonDifficulty.Advanced => "កម្រិតខ្ពស់",
                _ => "មិនស្គាល់"
            };
        }

        return difficulty switch
        {
            LessonDifficulty.Beginner => "Beginner",
            LessonDifficulty.Intermediate => "Intermediate",
            LessonDifficulty.Advanced => "Advanced",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Wraps the raw explanation string with a difficulty-appropriate intro phrase.
    /// </summary>
    private string GetDifficultyExplanation(string explanation, LessonDifficulty difficulty, bool isKhmer)
    {
        if (string.IsNullOrWhiteSpace(explanation))
        {
            return string.Empty;
        }

        return difficulty switch
        {
            LessonDifficulty.Beginner =>
                isKhmer
                    ? $"សូមចាប់ផ្តើមពីគោលគំនិតមូលដ្ឋាន៖\n\n{explanation}"
                    : $"Let's start with the basic idea:\n\n{explanation}",

            LessonDifficulty.Intermediate =>
                isKhmer
                    ? $"នេះជាការពន្យល់កម្រិតមធ្យម៖\n\n{explanation}"
                    : $"Here is the intermediate-level explanation:\n\n{explanation}",

            LessonDifficulty.Advanced =>
                isKhmer
                    ? $"នេះជាការពន្យល់លម្អិតសម្រាប់កម្រិតខ្ពស់៖\n\n{explanation}"
                    : $"Here is the detailed advanced-level explanation:\n\n{explanation}",

            _ => explanation
        };
    }

    /// <summary>
    /// Constructs a customized closing summary statement tailored to topic and difficulty level.
    /// </summary>
    private string BuildSummary(string title, LessonDifficulty difficulty, bool isKhmer)
    {
        if (isKhmer)
        {
            return difficulty switch
            {
                LessonDifficulty.Beginner =>
                    $"មេរៀននេះជួយអ្នកចាប់ផ្តើមយល់ពី {title}។ " +
                    "សូមផ្តោតលើគោលគំនិតសំខាន់ៗ និងអនុវត្តជាមួយឧទាហរណ៍។",

                LessonDifficulty.Intermediate =>
                    $"មេរៀននេះជួយអ្នកពង្រឹងការយល់ដឹងអំពី {title}។ " +
                    "សូមពិនិត្យជំហាន និងអនុវត្តជាមួយលំហាត់។",

                LessonDifficulty.Advanced =>
                    $"មេរៀននេះជួយអ្នកយល់ជ្រៅអំពី {title}។ " +
                    "សូមផ្តោតលើគោលការណ៍ បច្ចេកទេស និងករណីពិសេស។",

                _ => $"មេរៀននេះជួយអ្នកយល់ពី {title}។"
            };
        }

        return difficulty switch
        {
            LessonDifficulty.Beginner =>
                $"This lesson helps you build a basic understanding of {title}. " +
                "Focus on the main concepts and practice with examples.",

            LessonDifficulty.Intermediate =>
                $"This lesson helps you strengthen your understanding of {title}. " +
                "Review the steps and practice with exercises.",

            LessonDifficulty.Advanced =>
                $"This lesson helps you develop a deeper understanding of {title}. " +
                "Focus on the principles, technical details, and edge cases.",

            _ => $"This lesson helps you understand {title}."
        };
    }

    /// <summary>
    /// Extracts the core explanation text up to the start of the "Steps:" section.
    /// </summary>
    private string ExtractExplanation(string content)
    {
        var index = content.IndexOf("Steps:", StringComparison.OrdinalIgnoreCase);

        if (index > 0)
        {
            return content[..index].Trim();
        }

        return content.Trim();
    }

    /// <summary>
    /// Locates and extracts text for a given section title using predefined delimiters.
    /// </summary>
    private string ExtractSection(string content, string sectionName)
    {
        var start = content.IndexOf(sectionName, StringComparison.OrdinalIgnoreCase);

        if (start < 0)
        {
            return string.Empty;
        }

        start += sectionName.Length;

        if (start < content.Length && content[start] == ':')
        {
            start++;
        }

        // Section delimiters used as section endpoints
        string[] sections =
        {
            "Steps:",
            "Example:",
            "Time Complexity:",
            "Complexity:",
            "Summary:"
        };

        var end = content.Length;

        foreach (var section in sections)
        {
            var index = content.IndexOf(section, start, StringComparison.OrdinalIgnoreCase);

            if (index >= 0 && index < end)
            {
                end = index;
            }
        }

        return CleanText(content[start..end]);
    }

    /// <summary>
    /// Strips leading colons and extra whitespace from extracted section text.
    /// </summary>
    private string CleanText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        while (text.StartsWith(":"))
        {
            text = text[1..];
        }

        return text.Trim();
    }
}