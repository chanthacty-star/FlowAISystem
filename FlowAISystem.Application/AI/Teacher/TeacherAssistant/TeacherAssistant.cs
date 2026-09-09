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

    public async Task<TeacherAssistantResponse> AskAsync(TeacherAssistantRequest request) //add AskAsync to TeacherAssistantResponse
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

        if (isImprovementRequest) 
        {
            int lessonId;

            if (request.LessonId.HasValue && 
                request.LessonId.Value > 0)

            {
                lessonId = request.LessonId.Value;
            }
            else
            {
                var titleHint =
                    ExtractLessonTitleHint(request.Message);

                if (string.IsNullOrWhiteSpace(titleHint))
                {
                    return new TeacherAssistantResponse
                    {
                        Message =
                            "Whice Lesson would you like me to improve?" +
                            "Please include its title.",
                        Capability = "LessonImprovement" 
                    };
                }

                var matches =
                    await _lessonKnowledgeService.GetAllAsync(
                        new LessonKnowledgeSearchDto
                        {
                            SearchTerm = titleHint,
                            PageNumber = 1,
                            PageSize = 5

                        });

                if (matches.Count == 0) // mean user type lesson out of lessonKnowledge
                {
                    return new TeacherAssistantResponse
                    {
                        Message =
                            $"I Couldn't find a lesson titile \"{titleHint}\". " +
                            "Please check the title lesson again.",
                        Capability = "LessonImprovement"
                    };
                }

                if(matches.Count > 1)
                {
                    var options =
                        string.Join(
                            "\n",
                            matches.Select(
                                match => $"* {match.Title}"));
                    return new TeacherAssistantResponse
                    {
                        Message =
                            $"I found a few lessons matching \"{titleHint}\":\n\n" +
                            $"{options}\n\n" +
                            "Could you tell me the exact title you mean?",
                        Capability = "LessonImprovement"
                    };

                }
                lessonId = matches[0].Id; // what this statement


            } //end else condition
            var result =
                await _lessonEnhancementService
                    .ImproveAsync(lessonId);//maybe have other

            if (result == null)
            {
                return new TeacherAssistantResponse
                {
                    Message =
                        "I Could not find lesson to imprve.",
                    Capability = "LessonImprovement",
                    LessonId = lessonId

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
                              $"* [{suggestion.Priority}]" +
                              $"{suggestion.Area}: " +
                              $"{suggestion.Message}"));

            return new TeacherAssistantResponse
            {
                Message = message,
                Capability = "LessonImprovement",
                LessonId = lessonId
            };
        }
        return new TeacherAssistantResponse
        {
            Message =
                "I Can help you with your lesson. " +
                "Try to ask me to improve a lesson. ",
            Capability = "LessonImprovement"
        };
    }
    //Heuistic only: strips common instruction/fillter words so the
    // remainder is a reasonable guess at the lesson title the teacher
    //typed. the actual matching is delegated entirely to
    //IlessonKnowledgeService.GetAllAsync 's SearchTerm - this method
    ////never talds to a repository or database directly.'

    private static readonly string[] ImprovementFillerWords =
    {
        "improve", "please", "can", "you", "could", "would",
        "my", "the", "a", "an", "lesson", "for", "on", "about",
        "to", "of", "our"
    };
    ///
    private static string ExtractLessonTitleHint(string message)
    {
        var words = message
            .Split(
                new[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.Trim('.', ',', '!', '?', '"', '\''))
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word =>
                !ImprovementFillerWords.Contains(
                    word,
                    StringComparer.OrdinalIgnoreCase))
            .ToList();

        return string.Join(' ', words);
    }

    public async Task<TeacherAssistantResponse> ApplySuggestionsAsync(
        int lessonId)
    {
        if (lessonId == null)
        {
            return new TeacherAssistantResponse
            {
                Message =
                 "Invalid lesson to apply improvements to.",
                Capability = "LessonImprovement"
            };
        }
        Console.WriteLine(
            $"[Teacher AI Assistant] Applying Suggestions to lesson {lessonId}.");

        var lesson =
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "I could not find lesson to update",
                Capability = "LessonImprovement",
                LessonId = lessonId
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
                    "I Could not generate the lesson improvemet. ",
                Capability = "Lessonmprovement",
                LessonId = lessonId
            };
        }

        if (string.IsNullOrWhiteSpace(result.ImprovedContent))
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "There an no improveent change to apply(meand user enter nothing)",
                Capability = "Lessonimprovement",
                LessonId = lessonId
            };

        }

        //prepar every existing field from the load lesson;
        //only Content is replaced by AI improvement result.
        //
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
            Order = lesson.Order,
            TeacherId = lesson.TeacherId,
            CourseOfferingId = lesson.CourseOfferingId,
            ReferenceUrl = lesson.ReferenceUrl,
            AttachmentPath = lesson.AttachmentPath,
            IsActive = lesson.IsActive,
        };

        try
        {
            await _lessonKnowledgeService
                .UpdateAsync(updateDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[Teacher AI Assistant] Failed to update lesson {lessonId}: {ex.Message}");

            return new TeacherAssistantResponse
            {
                Message =
                    "I generated the improvement, but saving it to the lesson faile. Please try to again.",
                Capability = "LessonImprovement",
                LessonId = lessonId
            };
        }
        Console.WriteLine(
            $"[Teacher AI Assistant] Lesson {lessonId} updated successfully.");

        return new TeacherAssistantResponse
        {
            Message =
                $"Your lesson \"{lesson.Title}\" has been updated",
            Capability = "LessonImprovement",
            LessonId = lessonId
        };

    } //end apply method

    public async Task<TeacherAssistantResponse> ReviewSuggestionsAsync(
        int lessonId)
    {
        if (lessonId <= 0)
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "I need a valid lesson to reviw.",
                Capability = "Review"
            };
        }
        var lesson = 
            await _lessonKnowledgeService
                .GetByIdAsync(lessonId);

        if (lesson == null)
        {
            return new TeacherAssistantResponse
            {
                Message =
                    "I couldn't find lesson to review. can you apply lesson first?",
                Capability = "Review",
                LessonId  = lessonId
            };
        }
        var result =
            await _lessonEnhancementService
                .ImproveAsync(lessonId);

        if (result == null || string.IsNullOrWhiteSpace(result.ImprovedContent))
        {
            return new TeacherAssistantResponse
            {
                Message =
                    $"Here is the current content of \"{lesson.Title}\":\n\n" +
                    lesson.Content +
                    "\n\nNo improved version is aviable yet. ", // so, the future need to create agent for lesson improve context, but I had LessonEnhancement for (pp, En, act)?
                Capability = "Lessonimprovement",
                LessonId = lessonId
            };

        }
        var message =
            $"Reviewing \"{lesson.Title}\" berfore you update it:\n\n " +
            "- Current content -\n" +
            lesson.Content + // take reall lesson to display
            "\n\n- Suggested updated content -\n" +
            result.ImprovedContent;

        return new TeacherAssistantResponse
        {
            Message = message,
            Capability = "Reviw",
            LessonId = lessonId
        };


    }
}