using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;


namespace FlowAISystem.Application.Services;


public class StudentAIService : IStudentAIService
{

    private readonly ILessonKnowledgeService _lessonService;



    public StudentAIService(
        ILessonKnowledgeService lessonService)
    {
        _lessonService = lessonService;
    }



    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {


        var question =
            request.Question
            .ToLower()
            .Trim();



        string answer;

        string source;

        decimal confidence = 0.50m;



        // ==================================================
        // 1. Search Teacher Lesson Knowledge
        // ==================================================

        var lessons =
            await _lessonService
                .SearchAsync(question);



        if (lessons.Any())
        {

            var lesson =
                lessons.First();



            answer =
                $"Based on your teacher's lesson '{lesson.Title}':\n\n" +
                lesson.Content;



            source =
                "Teacher Lesson Knowledge";


            confidence =
                0.90m;



            return new StudentAIResponseDto
            {

                Answer = answer,

                Source = source,

                LessonId = lesson.Id,

                Confidence = confidence,

                CreatedAt = DateTime.UtcNow
            };

        }





        // ==================================================
        // 2. Student Data Knowledge
        // ==================================================

        if (question.Contains("gpa"))
        {

            answer =
                "I can help explain GPA calculation. " +
                "Your personal GPA data connection will be added soon.";


            source =
                "Student Academic Data";


        }



        else if (question.Contains("attendance"))
        {

            answer =
                "I can explain attendance records and attendance status.";


            source =
                "Attendance System";


        }



        else if (question.Contains("subject"))
        {

            answer =
                "I can help you understand enrolled subjects and courses.";


            source =
                "Course Information";


        }



        // ==================================================
        // 3. Default AI
        // ==================================================

        else
        {

            answer =
                "I am your Student AI Learning Assistant. " +
                "You can ask me about lessons, subjects, grades, " +
                "attendance, and university information.";


            source =
                "Student AI Knowledge Base";

        }




        return new StudentAIResponseDto
        {

            Answer = answer,

            Source = source,

            LessonId = request.LessonId,

            Confidence = confidence,

            CreatedAt = DateTime.UtcNow

        };


    }

}