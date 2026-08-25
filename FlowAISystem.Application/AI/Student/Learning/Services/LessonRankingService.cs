using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Services;

public class LessonRankingService : ILessonRankingService
{
    public LessonKnowledgeDto? Rank(
        string question,
        List<LessonKnowledgeDto> lessons)
    {
        if (string.IsNullOrWhiteSpace(question))
            return null;

        if (lessons == null || lessons.Count == 0)
            return null;

        var questionWords = ExtractWords(question);

        if (questionWords.Count == 0)
            return null;

        var rankedLessons = lessons
            .Select(lesson => new
            {
                Lesson = lesson,
                Score = CalculateScore(
                    lesson,
                    questionWords)
            })
            .OrderByDescending(x => x.Score)
            .ToList();

        return rankedLessons
            .FirstOrDefault(x => x.Score > 0)
            ?.Lesson;
    }

    // ==================================================
    // SCORE CALCULATION
    // ==================================================

    private int CalculateScore(
        LessonKnowledgeDto lesson,
        List<string> questionWords)
    {
        var score = 0;

        var title = Normalize(lesson.Title);
        var keywords = Normalize(lesson.Keywords);
        var category = Normalize(lesson.Category);
        var content = Normalize(lesson.Content);

        foreach (var word in questionWords)
        {
            // ==========================================
            // Title = Highest Priority
            // ==========================================

            if (ContainsKeyword(title, word))
            {
                score += 50;
            }

            // ==========================================
            // AI Keywords
            // ==========================================

            if (ContainsKeyword(keywords, word))
            {
                score += 30;
            }

            // ==========================================
            // Category
            // ==========================================

            if (ContainsKeyword(category, word))
            {
                score += 20;
            }

            // ==========================================
            // Content
            // ==========================================

            if (ContainsKeyword(content, word))
            {
                score += 10;
            }
        }

        return score;
    }

    // ==================================================
    // WORD EXTRACTION
    // ==================================================

    private List<string> ExtractWords(string text)
    {
        return text
            .ToLowerInvariant()
            .Split(
                new[]
                {
                    ' ',
                    ',',
                    '.',
                    '?',
                    '!',
                    ':',
                    ';',
                    '-',
                    '_',
                    '(',
                    ')',
                    '[',
                    ']',
                    '{',
                    '}',
                    '/',
                    '\\',
                    '\n',
                    '\r',
                    '\t'
                },
                StringSplitOptions.RemoveEmptyEntries)
            .Where(word => word.Length > 2)
            .Distinct()
            .ToList();
    }

    // ==================================================
    // NORMALIZATION
    // ==================================================

    private string Normalize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return text
            .Trim()
            .ToLowerInvariant();
    }

    // ==================================================
    // KEYWORD MATCHING
    // ==================================================

    private bool ContainsKeyword(
        string text,
        string keyword)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        if (string.IsNullOrWhiteSpace(keyword))
            return false;

        // Exact word/phrase match
        if (text.Contains(
            keyword,
            StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}