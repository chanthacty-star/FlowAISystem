//using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
//using FlowAISystem.Application.AI.Student.Conversation.Enums;
//using FlowAISystem.Application.AI.Student.Conversation.Models;
//using FlowAISystem.Application.AI.Student.Enums;
//using FlowAISystem.Application.AI.Student.Interfaces;
//using FlowAISystem.Application.AI.Student.Handlers;
//using FlowAISystem.Shared.DTOs.AI;

//namespace FlowAISystem.Application.AI.Student.Conversation.Services;

//public class ConversationHandler :
//    IConversationHandler,
//    IStudentAIWorkflowHandler
//{
//    private readonly IConversationContextBuilder _contextBuilder;
//    private readonly IConversationActionDetector _actionDetector;
//    private readonly IPromptBuilder _promptBuilder;
//    private readonly IStudentAIGenerator _aiGenerator;

//    public ConversationHandler(
//        IConversationContextBuilder contextBuilder,
//        IConversationActionDetector actionDetector,
//        IPromptBuilder promptBuilder,
//        IStudentAIGenerator aiGenerator)
//    {
//        _contextBuilder = contextBuilder;
//        _actionDetector = actionDetector;
//        _promptBuilder = promptBuilder;
//        _aiGenerator = aiGenerator;
//    }

//    public StudentIntent Intent
//        => StudentIntent.Conversation;

//    public async Task<StudentAIResponseDto> HandleAsync(
//        StudentAIRequestDto request)
//    {
//        return await HandleAsync(
//            request,
//            request.Language);
//    }

//    public async Task<StudentAIResponseDto> HandleAsync(
//        StudentAIRequestDto request,
//        string language)
//    {
//        if (!request.ConversationId.HasValue)
//        {
//            return new StudentAIResponseDto
//            {
//                Answer =
//                    "I need previous conversation context.",
//                Source = "Conversation",
//                Confidence = 0.50m,
//                CreatedAt = DateTime.UtcNow
//            };
//        }

//        // 1. Build conversation context
//        var context =
//            await _contextBuilder.BuildAsync(
//                request.ConversationId.Value,
//                request.Question);

//        if (!context.HasHistory)
//        {
//            return new StudentAIResponseDto
//            {
//                Answer =
//                    "No previous conversation found.",
//                Source = "Conversation",
//                Confidence = 0.40m,
//                CreatedAt = DateTime.UtcNow
//            };
//        }

//        // 2. Detect conversation action
//        var action =
//            _actionDetector.Detect(
//                request.Question);

//        // 3. Build prompt
//        var prompt =
//            BuildPrompt(
//                action,
//                context);

//        // 4. Generate AI response
//        var answer =
//            await _aiGenerator.GenerateAsync(prompt);

//        // 5. Return response
//        return new StudentAIResponseDto
//        {
//            Answer = answer,
//            Source = "Conversation Memory",
//            Confidence = 0.85m,
//            CreatedAt = DateTime.UtcNow
//        };
//    }

//    private string BuildPrompt(
//        ConversationAction action,
//        ConversationContext context)
//    {
//        return action switch
//        {
//            ConversationAction.ExplainMore =>
//                _promptBuilder.BuildExplainMore(context),

//            ConversationAction.ShowExample =>
//                _promptBuilder.BuildExample(context),

//            ConversationAction.CreateQuiz =>
//                _promptBuilder.BuildQuiz(context),

//            ConversationAction.Translate =>
//                _promptBuilder.BuildTranslation(context),

//            ConversationAction.Continue =>
//                _promptBuilder.BuildContinue(context),

//            ConversationAction.Compare =>
//                _promptBuilder.BuildComparison(context),

//            ConversationAction.Summarize =>
//                _promptBuilder.BuildSummary(context),

//            _ =>
//                _promptBuilder.BuildContinue(context)
//        };
//    }
//}
using FlowAISystem.Application.AI.Student.Conversation.Enums;
using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
using FlowAISystem.Application.AI.Student.Conversation.Models;
using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Handlers;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class ConversationHandler :
    IConversationHandler,
    IStudentAIWorkflowHandler
{
    private readonly IConversationContextBuilder _contextBuilder;
    private readonly IConversationActionDetector _actionDetector;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IStudentAIGenerator _aiGenerator;


    public ConversationHandler(
        IConversationContextBuilder contextBuilder,
        IConversationActionDetector actionDetector,
        IPromptBuilder promptBuilder,
        IStudentAIGenerator aiGenerator)
    {
        _contextBuilder = contextBuilder;
        _actionDetector = actionDetector;
        _promptBuilder = promptBuilder;
        _aiGenerator = aiGenerator;
    }


    // ==========================================
    // Workflow Handler
    // ==========================================

    public StudentIntent Intent =>
        StudentIntent.Conversation;


    // ==========================================
    // Default Entry Point
    // ==========================================

    public async Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request)
    {
        var language =
            NormalizeLanguage(request.Language);

        return await HandleAsync(
            request,
            language);
    }


    // ==========================================
    // Conversation Handler
    // ==========================================

    public async Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request,
        string language)
    {
        language =
            NormalizeLanguage(language);


        // ==========================================
        // 1. Validate Conversation
        // ==========================================

        if (!request.ConversationId.HasValue)
        {
            return CreateResponse(
                GetText(
                    language,
                    "ខ្ញុំត្រូវការបរិបទនៃការសន្ទនាមុន។",
                    "I need the previous conversation context."),
                "Conversation",
                0.50m);
        }


        // ==========================================
        // 2. Build Conversation Context
        // ==========================================

        var context =
            await _contextBuilder.BuildAsync(
                request.ConversationId.Value,
                request.Question);


        if (!context.HasHistory)
        {
            return CreateResponse(
                GetText(
                    language,
                    "រកមិនឃើញប្រវត្តិសន្ទនាមុនទេ។",
                    "No previous conversation was found."),
                "Conversation",
                0.40m);
        }


        // ==========================================
        // 3. Detect Conversation Action
        // ==========================================

        var action =
            _actionDetector.Detect(
                request.Question);


        // ==========================================
        // 4. Build Context-Aware Prompt
        // ==========================================

        var prompt =
            BuildPrompt(
                action,
                context);


        // ==========================================
        // 5. Generate AI Response
        // ==========================================

        var answer =
            await _aiGenerator.GenerateAsync(
                prompt);


        // ==========================================
        // 6. Return Response
        // ==========================================

        return CreateResponse(
            answer,
            "Conversation AI",
            0.85m);
    }


    // ==========================================
    // Prompt Builder Router
    // ==========================================

    private string BuildPrompt(
        ConversationAction action,
        ConversationContext context)
    {
        return action switch
        {
            ConversationAction.ExplainMore =>
                _promptBuilder.BuildExplainMore(
                    context),

            ConversationAction.ShowExample =>
                _promptBuilder.BuildExample(
                    context),

            ConversationAction.CreateQuiz =>
                _promptBuilder.BuildQuiz(
                    context),

            ConversationAction.Translate =>
                _promptBuilder.BuildTranslation(
                    context),

            ConversationAction.Continue =>
                _promptBuilder.BuildContinue(
                    context),

            ConversationAction.Compare =>
                _promptBuilder.BuildComparison(
                    context),

            ConversationAction.Summarize =>
                _promptBuilder.BuildSummary(
                    context),

            _ =>
                _promptBuilder.BuildContinue(
                    context)
        };
    }


    // ==========================================
    // Response Builder
    // ==========================================

    private StudentAIResponseDto CreateResponse(
        string answer,
        string source,
        decimal confidence)
    {
        return new StudentAIResponseDto
        {
            Answer = answer,
            Source = source,
            Confidence = confidence,
            CreatedAt = DateTime.UtcNow
        };
    }


    // ==========================================
    // Language Normalization
    // ==========================================

    private string NormalizeLanguage(
        string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return "en-US";


        if (language.StartsWith(
            "km",
            StringComparison.OrdinalIgnoreCase))
        {
            return "km-KH";
        }


        return "en-US";
    }


    // ==========================================
    // Language Helper
    // ==========================================

    private string GetText(
        string language,
        string khmer,
        string english)
    {
        return NormalizeLanguage(language)
            == "km-KH"
            ? khmer
            : english;
    }
}
