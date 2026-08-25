using FlowAISystem.Application.AI.Student.Quiz.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Quiz.Interfaces;

public interface IQuizGenerator
{
    Task<QuizResult> GenerateAsync(
        LessonKnowledgeDto lesson,
        int questionCount = 5,
        string difficulty = "Beginner");
}