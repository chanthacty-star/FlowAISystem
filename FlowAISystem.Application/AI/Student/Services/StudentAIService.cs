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



    public StudentAIService(
        ILessonKnowledgeService lessonService,
        IStudentIntentDetector intentDetector,
        IKeywordExtractor keywordExtractor)
    {
        _lessonService = lessonService;
        _intentDetector = intentDetector;
        _keywordExtractor = keywordExtractor;
    }



    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {

        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return new StudentAIResponseDto
            {
                Answer = "Please enter your question.",
                Source = "Student AI",
                Confidence = 0.20m,
                CreatedAt = DateTime.UtcNow
            };
        }



        var question =
            request.Question
                .Trim();



        // ==========================================
        // 1. Detect Intent
        // ==========================================

        var intent =
            _intentDetector
                .Detect(question);



        // ==========================================
        // 2. Extract Keywords
        // ==========================================

        var keywords =
            _keywordExtractor
                .Extract(question);



        switch (intent)
        {

            // ======================================
            // Learning Questions
            // ======================================

            case StudentIntent.Learn:

            case StudentIntent.Conversation:


                var searchText =
                    string.Join(
                        " ",
                        keywords);



                var lessons =
                    await _lessonService
                        .SearchAsync(searchText);



                if (lessons.Any())
                {
                    var lesson =
                        lessons.First();



                    return new StudentAIResponseDto
                    {
                        Answer =
                            $"Based on your teacher's lesson '{lesson.Title}':\n\n" +
                            lesson.Content,


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
            // Academic Information
            // ======================================

            case StudentIntent.AcademicInformation:


                return new StudentAIResponseDto
                {
                    Answer =
                        "I can help you with GPA, grades, attendance, subjects, and schedules. " +
                        "Student academic data integration will be connected soon.",


                    Source =
                        "Student Academic System",


                    Confidence =
                        0.70m,


                    CreatedAt =
                        DateTime.UtcNow
                };



            // ======================================
            // Greeting
            // ======================================

            case StudentIntent.Greeting:


                return new StudentAIResponseDto
                {
                    Answer =
                        "Hello 👋 I am your FlowAI Learning Assistant. " +
                        "You can ask me about lessons, subjects, grades, attendance, and university topics.",


                    Source =
                        "Student AI",


                    Confidence =
                        1.00m,


                    CreatedAt =
                        DateTime.UtcNow
                };

        }



        // ==========================================
        // Default Response
        // ==========================================


        return new StudentAIResponseDto
        {
            Answer =
                "I am your Student AI Learning Assistant. " +
                "Try asking me about lessons, courses, GPA, attendance, or academic topics.",


            Source =
                "Student AI Knowledge Base",


            Confidence =
                0.50m,


            CreatedAt =
                DateTime.UtcNow
        };

    }
}