using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Quiz.Interfaces;
using FlowAISystem.Application.AI.Student.Quiz.Models;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;

public class QuizHandler : IStudentAIWorkflowHandler
{
    private readonly ILessonKnowledgeService _lessonService;

    private readonly IQuizGenerator _quizGenerator;

    public QuizHandler(
        ILessonKnowledgeService lessonService,
        IQuizGenerator quizGenerator)
    {
        _lessonService = lessonService;
        _quizGenerator = quizGenerator;
    }

    public StudentIntent Intent =>
        StudentIntent.Quiz;

    public async Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request)
    {
        // 1. A quiz must have a real lesson.
        if (!request.LessonId.HasValue)
        {
            return new StudentAIResponseDto
            {
                Answer =
                    "Please select a lesson before starting a quiz.",

                Source = "Quiz",

                Confidence = 0.90m,

                CreatedAt = DateTime.UtcNow
            };
        }

        // 2. Get the actual selected lesson.
        var lesson =
            await _lessonService
                .GetLessonForAIAsync(
                    request.LessonId.Value);

        if (lesson == null)
        {
            return new StudentAIResponseDto
            {
                Answer =
                    "I could not find the selected lesson.",

                Source = "Quiz",

                Confidence = 0.80m,

                CreatedAt = DateTime.UtcNow
            };
        }

        // 3. Generate quiz from actual lesson.
        var quiz =
            await _quizGenerator.GenerateAsync(
                lesson,
                5,
                "Beginner");

        // 4. Convert quiz into current AI response.
        var answer =
            FormatQuiz(quiz);

        return new StudentAIResponseDto
        {
            Answer = answer,

            Source = "Quiz",

            Confidence = 0.90m,

            LessonId = lesson.Id,

            CreatedAt = DateTime.UtcNow
        };
    }

    private string FormatQuiz(
        QuizResult quiz)
    {
        var result =
            $"📝 {quiz.Topic}\n\n" +
            $"Difficulty: {quiz.Difficulty}\n\n";

        foreach (var question in quiz.Questions)
        {
            result +=
                $"{question.Number}. " +
                $"{question.Question}\n\n";

            foreach (var option in question.Options)
            {
                result +=
                    $"• {option}\n";
            }

            result +=
                $"\n✅ Answer: " +
                $"{question.CorrectAnswer}\n";

            result +=
                $"💡 Explanation: " +
                $"{question.Explanation}\n\n";
        }

        return result;
    }
}