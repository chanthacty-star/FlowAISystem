using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Services;


public class LessonRankingService
    : ILessonRankingService
{

    public LessonKnowledgeDto? Rank(
        string question,
        List<LessonKnowledgeDto> lessons)
    {

        if (string.IsNullOrWhiteSpace(question))
            return null;


        if (lessons == null || !lessons.Any())
            return null;



        var questionWords =
            ExtractWords(question);



        var rankedLessons =
            lessons
                .Select(lesson => new
                {
                    Lesson = lesson,

                    Score =
                        CalculateScore(
                            lesson,
                            questionWords)

                })
                .OrderByDescending(x => x.Score)
                .ToList();



        return rankedLessons
            .First()
            .Lesson;

    }





    private int CalculateScore(
        LessonKnowledgeDto lesson,
        List<string> keywords)
    {

        int score = 0;



        var title =
            lesson.Title
            .ToLower();



        var content =
            lesson.Content
            .ToLower();



        var lessonKeywords =
            lesson.Keywords
            .ToLower();




        foreach (var keyword in keywords)
        {

            // Title match
            if (title.Contains(keyword))
            {
                score += 50;
            }



            // AI keyword match
            if (lessonKeywords.Contains(keyword))
            {
                score += 30;
            }



            // Content match
            if (content.Contains(keyword))
            {
                score += 10;
            }

        }



        return score;

    }





    private List<string> ExtractWords(
        string text)
    {

        return text
            .ToLower()
            .Split(
                new[]
                {
                    ' ',
                    ',',
                    '.',
                    '?',
                    '!'
                },
                StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.Length > 2)
            .Distinct()
            .ToList();

    }

}