using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.Services;

public class StudentAIService : IStudentAIService
{

    public async Task<StudentAIResponseDto> AskAsync(
        StudentAIRequestDto request)
    {

        var question =
            request.Question
            .ToLower();


        string answer;


        string source;



        if (question.Contains("gpa"))
        {
            answer =
                "I can help you understand GPA calculation. Your personal GPA data will be connected soon.";

            source = "Student Academic Data";
        }


        else if (question.Contains("subject"))
        {
            answer =
                "I can help you understand your enrolled subjects and course information.";

            source = "Course Information";
        }


        else if (question.Contains("attendance"))
        {
            answer =
                "I can help you check attendance records and explain attendance status.";

            source = "Attendance System";
        }


        else if (question.Contains("lesson"))
        {
            answer =
                "I can explain lessons provided by your teachers. Lesson AI knowledge connection will be added soon.";

            source = "Teacher Lesson Knowledge Base";
        }


        else
        {
            answer =
                "I am your Student AI Learning Assistant. You can ask me about lessons, subjects, grades, attendance, and university information.";

            source = "Student AI Knowledge Base";
        }



        return new StudentAIResponseDto
        {
            Answer = answer,

            Source = source,

            LessonId = request.LessonId,

            Confidence = 0.80m
        };
    }

}