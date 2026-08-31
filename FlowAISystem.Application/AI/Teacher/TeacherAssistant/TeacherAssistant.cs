using FlowAISystem.Application.AI.Teacher.TeacherAssistant.Interfaces;
using FlowAISystem.Application.AI.Teacher.TeacherAssistant.Models;
using FlowAISystem.Application.AI.Teacher.LessonEnhancement.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Teacher.TeacherAssistant;

public class TeacherAssistant : ITeacherAssistant
{
    private readonly ILessonEnhancementService _lessonEnhancementService;
    private readonly ILessonKnowledgeService _lessonKnowledgeService;

    public TeacherAssistant(
        ILessonEnhancementService lessonEnhancementService,
        ILessonKnowledgeService lessonKnowledgeService)
    {
        _lessonEnhancementService = lessonEnhancementService;
        _lessonKnowledgeService = lessonKnowledgeService;
    }

    public async Task<TeacherAssistantResponse> AskAsync(
        TeacherAssistantRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return new TeacherAssistantResponse
            {
                Message = "Please enter a question or request.",
                Capability = "Question"
            };
        }

        Console.WriteLine(
            $"[Teacher AI Assistant] Request: '{request.Message}'");

        var isImprovementRequest =
            request.Message.Contains(
                "improve",
                StringComparison.OrdinalIgnoreCase);

        if (isImprovementRequest && request.LessonId.HasValue)
        {
            var result =
                await _lessonEnhancementService
                    .ImproveAsync(request.LessonId.Value);

            if (result == null)
            {
                return new TeacherAssistantResponse
                {
                    Message =
                        "I could not find the lesson to improve.",
                    Capability = "LessonImprovement"
                };
            }

            var message =
                result.Suggestions.Count == 0
                    ? "The lesson does not currently have any improvement suggestions."
                    : "I found these areas that could improve the lesson:\n\n" +
                      string.Join(
                          "\n",
                          result.Suggestions.Select(
                              suggestion =>
                                  $"• [{suggestion.Priority}] " +
                                  $"{suggestion.Area}: " +
                                  $"{suggestion.Message}"));

            return new TeacherAssistantResponse
            {
                Message = message,
                Capability = "LessonImprovement"
            };
        }

        return new TeacherAssistantResponse
        {
            Message =
                "I can help you with your lessons. " +
                "Try asking me to improve a lesson.",
            Capability = "Question"
        };
    }


    public async Task<TeacherAssistantResponse> ApplySuggestionsAsync(
        int lessonId)
    {
        Console.WriteLine(
            $"[Teacher AI Assistant] Applying suggestions to lesson {lessonId}.");

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "I couldn't find the lesson to update.",
                Capability = "LessonImprovement"
            };
        }

        var result =
            await _lessonEnhancementService
                .ImproveAsync(lessonId);

        if (result == null)
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "I couldn't generate the lesson improvement.",
                Capability = "LessonImprovement"
            };
        }

        if (string.IsNullOrWhiteSpace(result.ImprovedContent))
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "There are no improved lesson changes to apply.",
                Capability = "LessonImprovement"
            };
        }

        var updateDto = new UpdateLessonKnowledgeDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Description = lesson.Description,
            Content = result.ImprovedContent,
            Keywords = lesson.Keywords,
            Category = lesson.Category,
            Difficulty = lesson.Difficulty,
            ActivityType = lesson.ActivityType,
            TeacherId = lesson.TeacherId,
            CourseOfferingId = lesson.CourseOfferingId,
            ReferenceUrl = lesson.ReferenceUrl,
            AttachmentPath = lesson.AttachmentPath,
            IsActive = lesson.IsActive
        };

        await _lessonKnowledgeService
            .UpdateAsync(updateDto);

        Console.WriteLine(
            $"[Teacher AI Assistant] Lesson {lessonId} updated successfully.");

        return new TeacherAssistantResponse
        {
            Message =
                $"The improvements have been applied successfully to \"{lesson.Title}\".",
            Capability = "LessonImprovement"
        };
    }
}