using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;

public class StudentAIResponseFormatter
    : IStudentAIResponseFormatter
{

    public string Format(
        string title,
        string content,
        string language)
    {

        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }


        bool isKhmer =
            language == "km-KH";



        var result = new List<string>();



        // ==========================
        // Title
        // ==========================

        result.Add(
$"""
📘 {title}

"""
        );



        // ==========================
        // Explanation
        // ==========================

        result.Add(
$"""
🔎 {(isKhmer ? "ការពន្យល់" : "Explanation")}

{ExtractExplanation(content)}

"""
        );



        // ==========================
        // Steps
        // ==========================

        var steps =
            ExtractSection(
                content,
                "Steps");


        if (!string.IsNullOrWhiteSpace(steps))
        {

            result.Add(
$"""
⚙️ {(isKhmer ? "របៀបដំណើរការ" : "How It Works")}

{steps}

"""
            );

        }




        // ==========================
        // Example
        // ==========================

        var example =
            ExtractSection(
                content,
                "Example");


        if (!string.IsNullOrWhiteSpace(example))
        {

            result.Add(
$"""
💡 {(isKhmer ? "ឧទាហរណ៍" : "Example")}

{example}

"""
            );

        }




        // ==========================
        // Complexity
        // ==========================

        var complexity =
            ExtractSection(
                content,
                "Time Complexity");


        if (!string.IsNullOrWhiteSpace(complexity))
        {

            result.Add(
$"""
⏱ {(isKhmer ? "ភាពស្មុគស្មាញពេលវេលា" : "Time Complexity")}

{complexity}

"""
            );

        }




        // ==========================
        // Summary
        // ==========================

        result.Add(
$"""
✅ {(isKhmer ? "សង្ខេប" : "Summary")}

{(
    isKhmer
    ?
    $"មេរៀននេះជួយអ្នកយល់ពី {title}។ សូមពិនិត្យគំនិតសំខាន់ៗ និងអនុវត្តជាមួយឧទាហរណ៍។"
    :
    $"This lesson helps you understand {title}. Review the concepts and practice with examples."
)}
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



        var end =
            content.Length;



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






    private string CleanText(
        string text)
    {

        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;



        while (text.StartsWith(":"))
        {
            text = text[1..];
        }



        return text.Trim();

    }

}