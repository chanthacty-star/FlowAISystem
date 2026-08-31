using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.Models;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement.Expertise.English;

public class EnglishExpertise : ISubjectExpertise
{
    public string SubjectName => "English";

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

        var text =
            $"{lesson.Title} " +
            $"{lesson.Description} " +
            $"{lesson.Category} " +
            $"{lesson.Keywords}";

        return
            text.Contains(
                "english",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "grammar",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "vocabulary",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "speaking",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "listening",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "reading",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "writing",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "past simple",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "present simple",
                StringComparison.OrdinalIgnoreCase)
            ||
            text.Contains(
                "future",
                StringComparison.OrdinalIgnoreCase);
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
        // Content
        // ------------------------------------------

        var content =
            lesson.Content?.Trim()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(content))
        {
            result.MissingAreas.Add(
                "English content");

            result.Suggestions.Add(
                "Add an English explanation of the lesson topic.");

            result.ScoreAdjustment -= 10;

            return;
        }

        result.Strengths.Add(
            "English lesson contains learning content.");

        // ------------------------------------------
        // Explanation Depth
        // ------------------------------------------

        if (content.Length < 500)
        {
            result.MissingAreas.Add(
                "English Explanation Depth");

            result.Suggestions.Add(
                "Add more explanation and examples to help students understand the English concept.");

            result.ScoreAdjustment -= 5;
        }
        else
        {
            result.Strengths.Add(
                "English lesson provides reasonable explanation depth.");
        }

        // ------------------------------------------
        // Example
        // ------------------------------------------

        if (ContainsExample(content))
        {
            result.Strengths.Add(
                "English lesson contains examples.");
        }
        else
        {
            result.MissingAreas.Add(
                "English Examples");

            result.Suggestions.Add(
                "Add clear English examples related to the lesson topic.");

            result.ScoreAdjustment -= 5;
        }

        // ------------------------------------------
        // Practice
        // ------------------------------------------

        if (ContainsPractice(content))
        {
            result.Strengths.Add(
                "English lesson contains practice material.");
        }
        else
        {
            result.MissingAreas.Add(
                "English Practice");

            result.Suggestions.Add(
                "Add English practice exercises so students can apply the lesson.");

            result.ScoreAdjustment -= 5;
        }

        // ------------------------------------------
        // Vocabulary
        // ------------------------------------------

        if (ContainsVocabulary(
                lesson,
                content))
        {
            result.Strengths.Add(
                "English lesson includes vocabulary learning.");
        }
        else
        {
            result.MissingAreas.Add(
                "Vocabulary");

            result.Suggestions.Add(
                "Consider adding important vocabulary related to the lesson.");

            result.ScoreAdjustment -= 3;
        }

        // ------------------------------------------
        // Grammar
        // ------------------------------------------

        if (ContainsGrammar(
                lesson,
                content))
        {
            result.Strengths.Add(
                "English lesson includes grammar information.");
        }

        // ------------------------------------------
        // Final protection
        // ------------------------------------------

        result.ScoreAdjustment =
            Math.Clamp(
                result.ScoreAdjustment,
                -20,
                10);
    }

    // ==================================================
    // Helpers
    // ==================================================

    private static bool ContainsExample(
        string content)
    {
        return
            content.Contains(
                "example",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "for example",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "examples",
                StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsPractice(
        string content)
    {
        return
            content.Contains(
                "practice",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "exercise",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "exercises",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "activity",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "activities",
                StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsVocabulary(
        LessonKnowledgeDto lesson,
        string content)
    {
        return
            lesson.Title.Contains(
                "vocabulary",
                StringComparison.OrdinalIgnoreCase)
            ||
            lesson.Keywords.Contains(
                "vocabulary",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "vocabulary",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "new words",
                StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsGrammar(
        LessonKnowledgeDto lesson,
        string content)
    {
        return
            lesson.Title.Contains(
                "grammar",
                StringComparison.OrdinalIgnoreCase)
            ||
            lesson.Keywords.Contains(
                "grammar",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "grammar",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "tense",
                StringComparison.OrdinalIgnoreCase)
            ||
            content.Contains(
                "verb",
                StringComparison.OrdinalIgnoreCase);
    }
    //Improve method after analysis
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
                Area = "English Content",
                Message = "Add an English explanation with clear examples.",
                Priority = "High"
            });

            return;
        }

        if (!content.Contains(
                "example",
                StringComparison.OrdinalIgnoreCase))
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "English Examples",
                Message = "Add clear English examples to demonstrate the language concept.",
                Priority = "Medium"
            });
        }

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
                Area = "English Practice",
                Message = "Add a short language practice exercise for students.",
                Priority = "Medium"
            });
        }

        if (content.Length < 500)
        {
            result.Suggestions.Add(new LessonSuggestion
            {
                Area = "English Explanation",
                Message = "Add more explanation and examples around the language concept.",
                Priority = "Medium"
            });
        }
    }
}