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
        {
            return string.Empty;
        }


        var result = new List<string>();


        // Title
        result.Add(
$"""
📘 {title}

"""
        );



        // Explanation
        result.Add(
$"""
🔎 Explanation

{ExtractExplanation(content)}

"""
        );



        // Steps
        var steps =
            ExtractSection(
                content,
                "Steps");


        if (!string.IsNullOrWhiteSpace(steps))
        {
            result.Add(
$"""
⚙️ How It Works

{steps}

"""
            );
        }




        // Example
        var example =
            ExtractSection(
                content,
                "Example");


        if (!string.IsNullOrWhiteSpace(example))
        {
            result.Add(
$"""
💡 Example

{example}

"""
            );
        }





        // Complexity
        var complexity =
            ExtractSection(
                content,
                "Time Complexity");


        if (!string.IsNullOrWhiteSpace(complexity))
        {
            result.Add(
$"""
⏱ Time Complexity

{complexity}

"""
            );
        }




        // Summary

        result.Add(
$"""
✅ Summary

This lesson helps you understand {title}.
Review the concepts and practice with examples.
"""
        );


        return string.Join(
            Environment.NewLine,
            result);

    }





    private string ExtractExplanation(
        string content)
    {

        var index =
            content.IndexOf(
                "Steps:",
                StringComparison.OrdinalIgnoreCase);



        if (index > 0)
        {
            return content
                .Substring(0, index)
                .Trim();
        }


        return content.Trim();

    }



    // format the secttion that not conatin in lesson 
    private string ExtractSection(
        string content,
        string sectionName)
    {
        var start =
            content.IndexOf(
                sectionName,
                StringComparison.OrdinalIgnoreCase);


        if (start < 0)
            return string.Empty;



        start += sectionName.Length;



        // Remove ":" after section title
        if (start < content.Length &&
           content[start] == ':')
        {
            start++;
        }



        string[] sections =
        {
        "Steps:",
        "Example:",
        "Time Complexity:",
        "Complexity:",
        "Summary:"
    };



        var end = content.Length;



        foreach (var section in sections)
        {
            var index =
                content.IndexOf(
                    section,
                    start,
                    StringComparison.OrdinalIgnoreCase);



            if (index >= 0 && index < end)
            {
                end = index;
            }
        }



        return CleanText(
            content[start..end]);
    }
    // clean text
    private string CleanText(string text)
    {

        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;


        while (text.StartsWith(":"))
        {
            text = text[1..];
        }


        return text
            .Trim();

    }

}