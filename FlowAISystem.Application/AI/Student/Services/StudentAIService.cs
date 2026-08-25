//using FlowAISystem.Application.AI.Student.Enums;
//using FlowAISystem.Application.AI.Student.Interfaces;
//using FlowAISystem.Application.Interfaces.Services;
//using FlowAISystem.Shared.DTOs.AI;
//using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
////using FlowAISystem.Application.AI.Student.Conversation.Services;
//using FlowAISystem.Application.AI.Student.Conversation.Models;

//namespace FlowAISystem.Application.AI.Student.Services;

//public class StudentAIService : IStudentAIService
//{
//    private readonly ILessonKnowledgeService _lessonService;
//    private readonly IStudentIntentDetector _intentDetector;
//    private readonly IKeywordExtractor _keywordExtractor;
//    private readonly IStudentAIResponseFormatter _formatter;
//    private readonly ILessonRankingService _lessonRankingService;
//    private readonly IConversationActionDetector _actionDetector;

//    //private readonly IStudentConversationService _conversationService;
//    private readonly IConversationContextBuilder _contextBuilder;

//    public StudentAIService(
//        ILessonKnowledgeService lessonService,
//        IStudentIntentDetector intentDetector,
//        IKeywordExtractor keywordExtractor,
//        IStudentAIResponseFormatter formatter,
//        ILessonRankingService lessonRankingService,
//          //IStudentConversationService conversationService,
//        IConversationContextBuilder contextBuilder
//        IConversationActionDetector actionDetector,


//        )
//    {
//        _lessonService = lessonService;
//        _intentDetector = intentDetector;
//        _keywordExtractor = keywordExtractor;
//        _formatter = formatter;
//        _lessonRankingService = lessonRankingService;
//        //_conversationService = conversationService;
//        _contextBuilder = contextBuilder;
//        _actionDetector = actionDetector;
//    }

//    public async Task<StudentAIResponseDto> AskAsync(StudentAIRequestDto request)
//    {
//        if (string.IsNullOrWhiteSpace(request.Question))
//        {
//            return CreateResponse(
//                GetText(request.Language, "សូមបញ្ចូលសំណួររបស់អ្នក។", "Please enter your question."),
//                "Student AI",
//                0.20m);
//        }

//        var question = request.Question.Trim();
//        var language = NormalizeLanguage(request.Language);

//        Console.WriteLine($"Student Question: {question}");
//        Console.WriteLine($"Student Language: {language}");

//        // ==========================================
//        // Khmer Greeting Safety Check
//        // ==========================================
//        if (question.Contains("សួស្តី") || question.Contains("ជំរាបសួរ"))
//        {
//            return CreateResponse(
//                GetText(
//                    language,
//                    "សួស្តី 👋 ខ្ញុំជា FlowAI Learning Assistant។ តើខ្ញុំអាចជួយអ្នកអ្វីបានខ្លះ?",
//                    "Hello 👋 I am FlowAI Learning Assistant. How can I help you today?"),
//                "Student AI",
//                1.00m);
//        }

//        // ==========================================
//        // 1. Detect Intent
//        // ==========================================
//        var intent = _intentDetector.Detect(question);

//        // ==========================================
//        // 2. Extract Keywords
//        // ==========================================
//        var keywords = _keywordExtractor.Extract(question);

//        switch (intent)
//        {

//            case StudentIntent.Quiz:
//                return await HandleQuizAsync(...);

//            case StudentIntent.Recommendation:
//                return await HandleRecommendationAsync(...);

//            case StudentIntent.Learn:
//                return await HandleLearnAsync(question, keywords, language);

//            case StudentIntent.Conversation:
//                return await HandleConversationAsync(request, language);

//            case StudentIntent.AcademicInformation:
//                return CreateResponse(
//                    GetText(
//                        language,
//                        "ខ្ញុំអាចជួយអ្នកអំពី GPA ពិន្ទុ វត្តមាន មុខវិជ្ជា និងកាលវិភាគ។ ប្រព័ន្ធព័ត៌មាននិស្សិតនឹងភ្ជាប់នៅពេលក្រោយ។",
//                        "I can help you with GPA, grades, attendance, subjects, and schedules. Student academic data integration will be connected soon."),
//                    "Student Academic System",
//                    0.70m);

//            case StudentIntent.Greeting:
//                return CreateResponse(
//                    GetText(
//                        language,
//                        "សួស្តី 👋 ខ្ញុំជា FlowAI Learning Assistant។ អ្នកអាចសួរខ្ញុំអំពីមេរៀន មុខវិជ្ជា ពិន្ទុ វត្តមាន និងប្រធានបទសិក្សា។",
//                        "Hello 👋 I am your FlowAI Learning Assistant. You can ask me about lessons, subjects, grades, attendance, and university topics."),
//                    "Student AI",
//                    1.00m);

//            case StudentIntent.ThankYou:
//                return CreateResponse(
//                    GetText(
//                        language,
//                        "មិនអីទេ 😊 ខ្ញុំរីករាយដែលអាចជួយអ្នកក្នុងការសិក្សា។ សូមសួរខ្ញុំបានគ្រប់ពេល។",
//                        "You're welcome 😊. I am happy to help you learn. Feel free to ask me anytime."),
//                    "Student AI",
//                    1.00m);
//        }

//        // ==========================================
//        // Default
//        // ==========================================
//        return CreateResponse(
//            GetText(
//                language,
//                "ខ្ញុំជា Student AI Learning Assistant។ អ្នកអាចសួរខ្ញុំអំពីមេរៀន វគ្គសិក្សា GPA វត្តមាន ឬប្រធានបទសិក្សា។",
//                "I am your Student AI Learning Assistant. Try asking me about lessons, courses, GPA, attendance, or academic topics."),
//            "Student AI Knowledge Base",
//            0.50m);
//    }

//    // ==========================================
//    // Learn Intent Handler
//    // ==========================================
//    private async Task<StudentAIResponseDto> HandleLearnAsync(
//        string question,
//        IEnumerable<string> keywords,
//        string language)
//    {
//        var searchText = string.Join(" ", keywords);

//        var lessons = await _lessonService.SearchAsync(searchText);

//        var lesson = _lessonRankingService.Rank(question, lessons);

//        if (lesson != null)
//        {
//            return new StudentAIResponseDto
//            {
//                Answer = _formatter.Format(lesson.Title, lesson.Content, language),
//                Source = "Teacher Lesson Knowledge",
//                LessonId = lesson.Id,
//                Confidence = 0.90m,
//                CreatedAt = DateTime.UtcNow
//            };
//        }

//        return CreateResponse(
//            GetText(
//                language,
//                "រកមិនឃើញមេរៀនដែលពាក់ព័ន្ធនឹងសំណួររបស់អ្នកទេ។ សូមព្យាយាមសួរម្តងទៀត។",
//                "I couldn't find a lesson matching your question. Could you try rephrasing it?"),
//            "Student AI Knowledge Base",
//            0.40m);
//    }

//    // ==========================================
//    // Normalize Language
//    // ==========================================
//    private string NormalizeLanguage(string? language)
//    {
//        if (string.IsNullOrWhiteSpace(language))
//            return "en-US";

//        if (language.StartsWith("km", StringComparison.OrdinalIgnoreCase))
//            return "km-KH";

//        return "en-US";
//    }

//    // ==========================================
//    // Language Helper
//    // ==========================================
//    private string GetText(string? language, string khmer, string english)
//        => NormalizeLanguage(language) == "km-KH" ? khmer : english;

//    // ==========================================
//    // Response Builder
//    // ==========================================
//    private StudentAIResponseDto CreateResponse(string answer, string source, decimal confidence)
//        => new StudentAIResponseDto
//        {
//            Answer = answer,
//            Source = source,
//            Confidence = confidence,
//            CreatedAt = DateTime.UtcNow
//        };

//    // ==========================================
//    // Conversation Intent Handler
//    // ==========================================
//    private async Task<StudentAIResponseDto> HandleConversationAsync(
//            StudentAIRequestDto request,
//            string language)
//        {
//            if (context.PreviousAssistantMessage == null)
//            {
//                return CreateResponse(
//                    GetText(
//                        language,
//                        "ខ្ញុំមិនទាន់មានចម្លើយមុនទេ។",
//                        "I don't have a previous AI answer yet."),
//                    "Conversation",
//                    0.5m);
//            }
//            if (!request.ConversationId.HasValue)
//                {
//                    return CreateResponse(
//                        GetText(
//                            language,
//                            "សូមសួរសំណួរមុនសិន។",
//                            "Please ask a question first."),
//                        "Student AI",
//                        0.5m);
//                }

//                var context =
//                    await _contextBuilder.BuildAsync(
//                        request.ConversationId.Value,
//                        request.Question);

//                if (!context.HasHistory)
//                {
//                    return CreateResponse(
//                        GetText(
//                            language,
//                            "ខ្ញុំត្រូវការបរិបទពីមុន។",
//                            "I need previous context."),
//                        "Conversation",
//                        0.5m);
//                }
//            var action =
//                _actionDetector.Detect(request.Question);
//            switch (action)
//                {
//                    case ConversationAction.ExplainMore:

//                        ...

//                    case ConversationAction.ShowExample:

//                                ...

//                    case ConversationAction.CreateQuiz:

//                                ...

//                    case ConversationAction.Translate:

//                                ...

//                    default:

//                        ...
//            }

//           return CreateResponse(
//                   answer,
//                   "Conversation Memory",
//                   0.85m);
//       }
//}

using FlowAISystem.Application.AI.Student.Handlers;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.AI.Student.Response;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Services;


public class StudentAIService : IStudentAIService
{

    private readonly IStudentIntentDetector _intentDetector;

    private readonly IEnumerable<IStudentAIWorkflowHandler> _handlers;

    private readonly IStudentAIResponseBuilder _responseBuilder;



    public StudentAIService(
        IStudentIntentDetector intentDetector,
        IEnumerable<IStudentAIWorkflowHandler> handlers,
        IStudentAIResponseBuilder responseBuilder)
    {
        _intentDetector = intentDetector;
        _handlers = handlers;
        _responseBuilder = responseBuilder;
    }



    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {

        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return _responseBuilder.Create(
                GetText(
                    request.Language,
                    "សូមបញ្ចូលសំណួររបស់អ្នក។",
                    "Please enter your question."),
                "Student AI",
                0.20m);
        }



        var question =
            request.Question.Trim();



        var intent =
            _intentDetector.Detect(question);



        var handler =
            _handlers.FirstOrDefault(
                h => h.Intent == intent);



        if (handler == null)
        {
            return _responseBuilder.Create(
                GetText(
                    request.Language,
                    "ខ្ញុំមិនទាន់អាចឆ្លើយសំណួរនេះបានទេ។",
                    "I cannot handle this request yet."),
                "Student AI",
                0.50m);
        }



        return await handler.HandleAsync(request);
    }




    private string GetText(
        string? language,
        string khmer,
        string english)
    {
        if (!string.IsNullOrWhiteSpace(language)
            && language.StartsWith(
                "km",
                StringComparison.OrdinalIgnoreCase))
        {
            return khmer;
        }

        return english;
    }
}