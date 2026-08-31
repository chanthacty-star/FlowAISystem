using System.Diagnostics;

using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Models;
using FlowAISystem.Application.Interfaces.Services;

namespace FlowAISystem.Application.AI.Teacher.LessonEnhancement;

public class LessonEnhancementService : ILessonEnhancementService
{
    private readonly ILessonKnowledgeService _lessonKnowledgeService;
    private readonly ILessonEnhancementAgent _lessonEnhancementAgent;

    public LessonEnhancementService(
        ILessonKnowledgeService lessonKnowledgeService,
        ILessonEnhancementAgent lessonEnhancementAgent)
    {
        _lessonKnowledgeService = lessonKnowledgeService;
        _lessonEnhancementAgent = lessonEnhancementAgent;
    }

    // ==================================================
    // Analyze Lesson
    // ==================================================

    public async Task<LessonAnalysisResult?> AnalyzeAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            throw new ArgumentException(
                "Invalid lesson id.",
                nameof(lessonId));
        }

        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine(
            $"[Teacher AI] Analyze started. LessonId={lessonId}");

        try
        {
            // ------------------------------------------
            // STEP 1: Load lesson
            // ------------------------------------------

            var databaseTimer = Stopwatch.StartNew();

            var lesson =
                await _lessonKnowledgeService
                    .GetByIdAsync(lessonId);

            databaseTimer.Stop();

            Console.WriteLine(
                $"[Teacher AI] Lesson loaded in " +
                $"{databaseTimer.ElapsedMilliseconds} ms.");


            if (lesson == null)
            {
                Console.WriteLine(
                    $"[Teacher AI] Lesson not found. LessonId={lessonId}");

                return null;
            }


            Console.WriteLine(
                $"[Teacher AI] Lesson: '{lesson.Title}'");


            // ------------------------------------------
            // STEP 2: Analyze lesson
            // ------------------------------------------

            var agentTimer = Stopwatch.StartNew();

            var result =
                await _lessonEnhancementAgent
                    .AnalyzeAsync(lesson);

            agentTimer.Stop();


            Console.WriteLine(
                $"[Teacher AI] Agent analysis completed in " +
                $"{agentTimer.ElapsedMilliseconds} ms.");


            // ------------------------------------------
            // STEP 3: Result information
            // ------------------------------------------

            if (result != null)
            {
                Console.WriteLine(
                    $"[Teacher AI] Score: {result.Score}");

                Console.WriteLine(
                    $"[Teacher AI] Strengths: " +
                    $"{result.Strengths.Count}");

                Console.WriteLine(
                    $"[Teacher AI] Missing Areas: " +
                    $"{result.MissingAreas.Count}");

                Console.WriteLine(
                    $"[Teacher AI] Suggestions: " +
                    $"{result.Suggestions.Count}");
            }


            stopwatch.Stop();

            Console.WriteLine(
                $"[Teacher AI] Analyze finished in " +
                $"{stopwatch.ElapsedMilliseconds} ms.");


            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            Console.WriteLine(
                $"[Teacher AI] Analyze FAILED after " +
                $"{stopwatch.ElapsedMilliseconds} ms.");

            Console.WriteLine(
                $"[Teacher AI] Error: {ex}");

            throw;
        }
    }


    // ==================================================
    // Improve Lesson
    // ==================================================

    public async Task<LessonImprovementResult?> ImproveAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            throw new ArgumentException(
                "Invalid lesson id.",
                nameof(lessonId));
        }

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return null;
        }

        return await _lessonEnhancementAgent
            .ImproveAsync(lesson);
    }


    // ==================================================
    // Generate Examples
    // ==================================================

    public async Task<string?> GenerateExamplesAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            throw new ArgumentException(
                "Invalid lesson id.",
                nameof(lessonId));
        }

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return null;
        }

        return await _lessonEnhancementAgent
            .GenerateExamplesAsync(lesson);
    }


    // ==================================================
    // Generate Practice
    // ==================================================

    public async Task<string?> GeneratePracticeAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            throw new ArgumentException(
                "Invalid lesson id.",
                nameof(lessonId));
        }

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return null;
        }

        return await _lessonEnhancementAgent
            .GeneratePracticeAsync(lesson);
    }


    // ==================================================
    // Generate Summary
    // ==================================================

    public async Task<string?> GenerateSummaryAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            throw new ArgumentException(
                "Invalid lesson id.",
                nameof(lessonId));
        }

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return null;
        }

        return await _lessonEnhancementAgent
            .GenerateSummaryAsync(lesson);
    }

    //public async Task<string?> ApllySuggestionAsync(
    //    int lessonId)
    //{
    //    Console.WriteLine($"[Teacher AI Suggestios: ] Applying Lesson for display. ");

    //    var lesson = await _lessonKnowledgeService.GetByIdAsync(lessonId);
    //    if (lesson == null)
    //    {
    //        return new TeacherAssistantRespont
    //        {
    //            Message = "I Could not find the respond for suggestion (Have no Knowledge yet.)",
    //            Capability = "Lesson Knowledge"
    //        };
    //    }
    //    var result = await _lessonEnhancementAgent;
    //    if (result == null)
    //    {
    //        return new TeacherAssistantRespont
    //        {
    //            Message = "I Couln't find the result to improve lesson yet (haven't Knowledge)",
    //            Capabilty = "Apply LessonSuggeton "
    //        };
    //    }

    //}

}