using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Programming;

public class ProgrammingExpertise : ISubjectExpertise
{
    // ==================================================
    // Identity
    // ==================================================

    public string SubjectName => "Programming";


    // ==================================================
    // Can Handle
    // ==================================================

    public bool CanHandle(
        LessonKnowledgeDto lesson)
    {
        if (lesson == null)
        {
            return false;
        }

        // ------------------------------------------
        // Category is the strongest signal
        // ------------------------------------------

        if (IsProgrammingCategory(
                lesson.Category))
        {
            return true;
        }

        // ------------------------------------------
        // Build routing text
        // ------------------------------------------

        var text =
            $"{lesson.Title} " +
            $"{lesson.Description} " +
            $"{lesson.Keywords}";

        // ------------------------------------------
        // Strong programming identifiers
        // ------------------------------------------

        return Contains(
                   text,
                   "programming")
               ||
               Contains(
                   text,
                   "c#")
               ||
               Contains(
                   text,
                   "csharp")
               ||
               Contains(
                   text,
                   "python")
               ||
               Contains(
                   text,
                   "java")
               ||
               Contains(
                   text,
                   "javascript")
               ||
               Contains(
                   text,
                   "typescript")
               ||
               Contains(
                   text,
                   "software development")
               ||
               Contains(
                   text,
                   "software engineering")
               ||
               Contains(
                   text,
                   "computer programming");
    }


    // ==================================================
    // Analyze
    // ==================================================

    public void Analyze(
        LessonKnowledgeDto lesson,
        SubjectAnalysisResult result)
    {
        if (lesson == null)
        {
            return;
        }

        // ------------------------------------------
        // Programming Content
        // ------------------------------------------

        var content =
            lesson.Content?.Trim()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(content))
        {
            result.MissingAreas.Add(
                "Programming content");

            result.Suggestions.Add(
                "Add programming explanation and code examples.");

            result.ScoreAdjustment -= 10;

            return;
        }

        result.Strengths.Add(
            "Programming lesson contains learning content.");


        // ------------------------------------------
        // Code Example
        // ------------------------------------------

        if (content.Contains(
                "```",
                StringComparison.OrdinalIgnoreCase))
        {
            result.Strengths.Add(
                "Programming lesson contains a code example.");
        }
        else
        {
            result.MissingAreas.Add(
                "Code Example");

            result.Suggestions.Add(
                "Add at least one code example.");

            result.ScoreAdjustment -= 5;
        }


        // ------------------------------------------
        // Explanation Depth
        // ------------------------------------------

        if (content.Length < 500)
        {
            result.MissingAreas.Add(
                "Programming Explanation Depth");

            result.Suggestions.Add(
                "Add more explanation around the programming concept.");

            result.ScoreAdjustment -= 5;
        }
        else
        {
            result.Strengths.Add(
                "Programming lesson provides reasonable explanation depth.");
        }


        // ------------------------------------------
        // Practice
        // ------------------------------------------

        if (content.Contains(
                "practice",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "exercise",
                StringComparison.OrdinalIgnoreCase))
        {
            result.Strengths.Add(
                "Programming lesson contains practice material.");
        }
        else
        {
            result.MissingAreas.Add(
                "Programming Practice");

            result.Suggestions.Add(
                "Add a programming exercise for students.");

            result.ScoreAdjustment -= 5;
        }


        // ------------------------------------------
        // Final Protection
        // ------------------------------------------

        result.ScoreAdjustment =
            Math.Clamp(
                result.ScoreAdjustment,
                -20,
                10);
    }
    //Improve method after analyzsis 
    public void Improve(
    LessonKnowledgeDto lesson,
    LessonImprovementResult result)
    {
        if (lesson == null || result == null)
        {
            return;
        }

        var content =
            lesson.Content?.Trim()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(content))
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "Programming Content",
                Message = "Add programming explanation and code examples.",
                Priority = "High"
            });

            return;
        }

        // ------------------------------------------
        // Code Example
        // ------------------------------------------

        if (!content.Contains(
                "```",
                StringComparison.OrdinalIgnoreCase))
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "Programming Code Example",
                Message = "Add a programming code example that demonstrates the main concept.",
                Priority = "Medium"
            });
        }

        // ------------------------------------------
        // Programming Practice
        // ------------------------------------------

        if (!content.Contains(
                "practice",
                StringComparison.OrdinalIgnoreCase)
            &&
            !content.Contains(
                "exercise",
                StringComparison.OrdinalIgnoreCase))
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "Programming Practice",
                Message = "Add a short programming exercise so students can apply the concept.",
                Priority = "Medium"
            });
        }

        // ------------------------------------------
        // Explanation Depth
        // ------------------------------------------

        if (content.Length < 500)
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "Programming Explanation",
                Message = "Add more explanation around the programming concept.",
                Priority = "Medium"
            });
        }
    }

    // ==================================================
    // Helpers
    // ==================================================

    private static bool IsProgrammingCategory(
        string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return false;
        }

        return category.Contains(
                   "programming",
                   StringComparison.OrdinalIgnoreCase)
               ||
               category.Contains(
                   "computer science",
                   StringComparison.OrdinalIgnoreCase)
               ||
               category.Contains(
                   "software",
                   StringComparison.OrdinalIgnoreCase)
               ||
               category.Contains(
                   "coding",
                   StringComparison.OrdinalIgnoreCase);
    }


    private static bool Contains(
        string text,
        string value)
    {
        return text.Contains(
            value,
            StringComparison.OrdinalIgnoreCase);
    }
}