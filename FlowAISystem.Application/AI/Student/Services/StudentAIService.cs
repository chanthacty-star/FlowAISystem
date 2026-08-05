using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Services;

public class StudentAIService : IStudentAIService
{
    private readonly ILessonKnowledgeService _lessonService;
    private readonly IStudentIntentDetector _intentDetector;
    private readonly IKeywordExtractor _keywordExtractor;
    private readonly IStudentAIResponseFormatter _formatter;
    private readonly ILessonRankingService _lessonRankingService;


    public StudentAIService(
        ILessonKnowledgeService lessonService,
        IStudentIntentDetector intentDetector,
        IKeywordExtractor keywordExtractor,
        IStudentAIResponseFormatter formatter,
        ILessonRankingService lessonRankingService)
    {
        _lessonService = lessonService;
        _intentDetector = intentDetector;
        _keywordExtractor = keywordExtractor;
        _formatter = formatter;
        _lessonRankingService = lessonRankingService;
    }



    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {


        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return CreateResponse(

                GetText(
                    request.Language,

                    "សូមបញ្ចូលសំណួររបស់អ្នក។",

                    "Please enter your question."
                ),

                "Student AI",

                0.20m
            );
        }



        var question =
            request.Question.Trim();



        var language =
            NormalizeLanguage(
                request.Language
            );



        Console.WriteLine(
            $"Student Question: {question}"
        );


        Console.WriteLine(
            $"Student Language: {language}"
        );



        // ==========================================
        // Khmer Greeting Safety Check
        // ==========================================

        if (
            question.Contains("សួស្តី") ||
            question.Contains("ជំរាបសួរ")
        )
        {

            return CreateResponse(

                GetText(
                    language,

                    "សួស្តី 👋 ខ្ញុំជា FlowAI Learning Assistant។ តើខ្ញុំអាចជួយអ្នកអ្វីបានខ្លះ?",

                    "Hello 👋 I am FlowAI Learning Assistant. How can I help you today?"
                ),

                "Student AI",

                1.00m
            );

        }




        // ==========================================
        // 1. Detect Intent
        // ==========================================

        var intent =
            _intentDetector.Detect(question);




        // ==========================================
        // 2. Extract Keywords
        // ==========================================

        var keywords =
            _keywordExtractor.Extract(question);




        switch (intent)
        {


            // ======================================
            // Learning
            // ======================================

            case StudentIntent.Learn:

            case StudentIntent.Conversation:


                var searchText =
                    string.Join(
                        " ",
                        keywords
                    );



                var lessons =
                    await _lessonService
                    .SearchAsync(searchText);



                var lesson =
                    _lessonRankingService
                    .Rank(
                        question,
                        lessons
                    );



                if (lesson != null)
                {

                    return new StudentAIResponseDto
                    {
                        Answer =
                            _formatter.Format(
                                lesson.Title,
                                lesson.Content,
                                language
                            ),

                        Source =
                            "Teacher Lesson Knowledge",


                        LessonId =
                            lesson.Id,


                        Confidence =
                            0.90m,


                        CreatedAt =
                            DateTime.UtcNow
                    };

                }


                break;




            // ======================================
            // Academic
            // ======================================

            case StudentIntent.AcademicInformation:


                return CreateResponse(

                    GetText(
                        language,


                        "ខ្ញុំអាចជួយអ្នកអំពី GPA ពិន្ទុ វត្តមាន មុខវិជ្ជា និងកាលវិភាគ។ ប្រព័ន្ធព័ត៌មាននិស្សិតនឹងភ្ជាប់នៅពេលក្រោយ។",


                        "I can help you with GPA, grades, attendance, subjects, and schedules. Student academic data integration will be connected soon."
                    ),


                    "Student Academic System",


                    0.70m
                );






            // ======================================
            // Greeting
            // ======================================

            case StudentIntent.Greeting:


                return CreateResponse(

                    GetText(
                        language,


                        "សួស្តី 👋 ខ្ញុំជា FlowAI Learning Assistant។ អ្នកអាចសួរខ្ញុំអំពីមេរៀន មុខវិជ្ជា ពិន្ទុ វត្តមាន និងប្រធានបទសិក្សា។",


                        "Hello 👋 I am your FlowAI Learning Assistant. You can ask me about lessons, subjects, grades, attendance, and university topics."
                    ),


                    "Student AI",


                    1.00m
                );






            // ======================================
            // Thank You
            // ======================================

            case StudentIntent.ThankYou:


                return CreateResponse(

                    GetText(
                        language,


                        "មិនអីទេ 😊 ខ្ញុំរីករាយដែលអាចជួយអ្នកក្នុងការសិក្សា។ សូមសួរខ្ញុំបានគ្រប់ពេល។",


                        "You're welcome 😊. I am happy to help you learn. Feel free to ask me anytime."
                    ),


                    "Student AI",


                    1.00m
                );

        }




        // ==========================================
        // Default
        // ==========================================

        return CreateResponse(

            GetText(
                language,


                "ខ្ញុំជា Student AI Learning Assistant។ អ្នកអាចសួរខ្ញុំអំពីមេរៀន វគ្គសិក្សា GPA វត្តមាន ឬប្រធានបទសិក្សា។",


                "I am your Student AI Learning Assistant. Try asking me about lessons, courses, GPA, attendance, or academic topics."
            ),


            "Student AI Knowledge Base",


            0.50m
        );

    }






    // ==========================================
    // Normalize Language
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
        string? language,
        string khmer,
        string english)
    {

        return NormalizeLanguage(language)
            == "km-KH"

            ?

            khmer

            :

            english;

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

}