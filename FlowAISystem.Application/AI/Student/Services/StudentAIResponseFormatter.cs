using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;


public class StudentAIResponseFormatter
    : IStudentAIResponseFormatter
{

    public string Format(
        string title,
        string content)
    {

        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;


        var response =
$"""
📘 {title}


🔎 Explanation

{content}


✅ Summary

This lesson helps you understand {title}.
Review the key concepts and practice with examples.
""";


        return response;

    }

}