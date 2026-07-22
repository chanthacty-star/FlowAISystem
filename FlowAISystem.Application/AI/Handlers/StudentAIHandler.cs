using System.Text;
using System.Text.RegularExpressions;

using FlowAISystem.Application.AI.Memory;
using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.Interfaces.Repositories;

namespace FlowAISystem.Application.AI.Handlers;

public class StudentAIHandler : IStudentAIHandler
{
    private readonly IStudentRepository _studentRepository;
    private readonly AIConversationMemory _memory;// memory handler

    public async Task<string> HandleAsync(
        string question)
    {
        question = question.ToLower();

        var currentStudent =
            _memory.Get<Shared.DTOs.Students.StudentListItemDto>(
                AIMemoryKeys.CurrentStudent);

        if (currentStudent != null)
        {
            if (question.Contains("department"))
            {
                return $"{currentStudent.FullName} belongs to the {currentStudent.DepartmentName} department.";
            }

            if (question.Contains("email"))
            {
                return $"{currentStudent.FullName}'s email is {currentStudent.Email}.";
            }

            if (question.Contains("enrollment"))
            {
                return $"{currentStudent.FullName} enrolled on {currentStudent.EnrollmentDate}.";
            }

            if (question.Contains("student number"))
            {
                return $"{currentStudent.FullName}'s student number is {currentStudent.StudentNumber}.";
            }
        }

        // ==========================================
        // Count Students
        // ==========================================

        if (IsCountQuestion(question))
        {
            return await GetStudentCountAsync();
        }

        // ==========================================
        // Recent Students
        // ==========================================

        if (IsRecentQuestion(question))
        {
            return await GetRecentStudentsAsync();
        }

        // ==========================================
        // Find Student Number
        // Example:
        // find student ST001
        // ==========================================

        var studentNumber =
            ExtractStudentNumber(question);


        if (studentNumber != null)
        {
            return await FindStudentByNumberAsync(
                studentNumber);
        }
    
        // ==========================================
        // Find Student Name
        // Example:
        // find John
        // ==========================================

        var name =
            ExtractStudentName(question);


        if (name != null)
        {
            return await FindStudentByNameAsync(
                name);
        }



        return
            "I can help you with students. Try asking:\n\n" +
            "• How many students are there?\n" +
            "• Show recent students\n" +
            "• Find student ST001\n" +
            "• Find John";
    }

    // ==========================================
    // Count Detection
    // ==========================================

    private bool IsCountQuestion(
        string question)
    {
        return question.Contains("how many")
            ||
            question.Contains("count")
            ||
            question.Contains("total");
    }

    // ==========================================
    // Recent Detection
    // ==========================================

    private bool IsRecentQuestion(
        string question)
    {
        return question.Contains("recent")
            ||
            question.Contains("latest")
            ||
            question.Contains("new");
    }

    // ==========================================
    // Extract Student Number
    // ==========================================

    private string? ExtractStudentNumber(
        string question)
    {
        var match =
            Regex.Match(
                question,
                @"\b[a-z]{2}\d{3,}\b",
                RegexOptions.IgnoreCase);


        if (match.Success)
        {
            return match.Value.ToUpper();
        }


        return null;
    }

    // ==========================================
    // Extract Student Name
    // ==========================================

    private string? ExtractStudentName(
        string question)
    {
        var removeWords = new[]
        {
            "find",
            "student",
            "show",
            "get",
            "search",
            "information",
            "about"
        };


        var words =
            question
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
            .Where(x =>
                !removeWords.Contains(x))
            .ToList();


        if (words.Count == 0)
            return null;


        return string.Join(
            " ",
            words);
    }

    // ==========================================
    // Find By Student Number
    // ==========================================

    private async Task<string> FindStudentByNumberAsync(
        string studentNumber)
    {
        var student =
            await _studentRepository
                .FindByStudentNumberAsync(
                    studentNumber);


        if (student == null)
        {
            return
                $"I couldn't find student {studentNumber}.";
        }

        _memory.Set(
            AIMemoryKeys.CurrentStudent,
            student);// memmory 

        return FormatStudent(student);
    }

    // ==========================================
    // Find By Name
    // ==========================================

    private async Task<string> FindStudentByNameAsync(
        string name)
    {
        var student =
            await _studentRepository
                .FindByNameAsync(name);


        if (student == null)
        {
            return
                $"I couldn't find a student named {name}.";
        }

        _memory.Set(
            AIMemoryKeys.CurrentStudent,
            student); // memory

        return FormatStudent(student);
    }

    // ==========================================
    // Student Count
    // ==========================================

    private async Task<string> GetStudentCountAsync()
    {
        var total =
            await _studentRepository
                .GetCountAsync();


        return $"There are currently {total} students in FlowAISystem.";
    }

    // ==========================================
    // Recent Students
    // ==========================================

    private async Task<string> GetRecentStudentsAsync()
    {
        var students =
            await _studentRepository
                .GetRecentStudentsAsync();


        if (students.Count == 0)
        {
            return "No students found.";
        }



        var builder =
            new StringBuilder();


        builder.AppendLine(
            "Recent Students:\n");


        foreach (var student in students)
        {
            builder.AppendLine(
                $"• {student.StudentNumber} - {student.FullName}");
        }


        return builder.ToString();
    }

    // ==========================================
    // Format Student
    // ==========================================

    private string FormatStudent(
        Shared.DTOs.Students.StudentListItemDto student)
    {
        return
$"""
Student Information

Student Number : {student.StudentNumber}
Name           : {student.FullName}
Email          : {student.Email}
Department     : {student.DepartmentName}
Enrollment     : {student.EnrollmentDate}
""";
    }

    public StudentAIHandler(
        IStudentRepository studentRepository,
        AIConversationMemory memory)
    {
        _studentRepository = studentRepository;
        _memory = memory;
    }
}