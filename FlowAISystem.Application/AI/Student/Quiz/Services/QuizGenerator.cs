//using FlowAISystem.Application.AI.Student.Quiz.Interfaces;
//using FlowAISystem.Application.AI.Student.Quiz.Models;
//using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;
//using System.Text.RegularExpressions;

//namespace FlowAISystem.Application.AI.Student.Quiz.Services;

//public class QuizGenerator : IQuizGenerator
//{
//    public Task<QuizResult> GenerateAsync(
//        LessonKnowledgeDto lesson,
//        int questionCount = 5,
//        string difficulty = "Beginner")
//    {
//        if (lesson == null)
//        {
//            throw new ArgumentNullException(nameof(lesson));
//        }

//        if (questionCount <= 0)
//        {
//            questionCount = 5;
//        }

//        var result = new QuizResult
//        {
//            Topic = lesson.Title?.Trim() ?? "Lesson",
//            Difficulty = string.IsNullOrWhiteSpace(difficulty)
//                ? "Beginner"
//                : difficulty
//        };

//        var questions =
//            BuildQuestions(
//                lesson,
//                questionCount);

//        result.Questions = questions;

//        return Task.FromResult(result);
//    }


//    // ============================================================
//    // Build Questions
//    // ============================================================

//    private List<QuizQuestion> BuildQuestions(
//        LessonKnowledgeDto lesson,
//        int questionCount)
//    {
//        var questions =
//            new List<QuizQuestion>();

//        var topic =
//            CleanText(lesson.Title);

//        var content =
//            CleanLessonContent(
//                lesson.Content);

//        var sentences =
//            ExtractSentences(content);

//        // --------------------------------------------------------
//        // Question 1 - Main Topic
//        // --------------------------------------------------------

//        questions.Add(
//            new QuizQuestion
//            {
//                Number = 1,

//                Question =
//                    $"What is the main topic of the lesson \"{topic}\"?",

//                Options =
//                    ShuffleOptions(
//                        topic,
//                        "Computer Networking",
//                        "Operating Systems",
//                        "Database Administration"),

//                CorrectAnswer =
//                    topic,

//                Explanation =
//                    $"The lesson is about {topic}."
//            });


//        // --------------------------------------------------------
//        // Question 2 - Definition / Main Concept
//        // --------------------------------------------------------

//        var definition =
//            FindDefinitionSentence(
//                sentences,
//                topic);

//        questions.Add(
//            new QuizQuestion
//            {
//                Number = 2,

//                Question =
//                    $"Which statement best describes {topic}?",

//                Options =
//                    ShuffleOptions(
//                        definition,
//                        "It is only related to computer hardware.",
//                        "It has no practical use in programming.",
//                        "It is unrelated to software development."),

//                CorrectAnswer =
//                    definition,

//                Explanation =
//                    "The correct answer is taken from the lesson material."
//            });


//        // --------------------------------------------------------
//        // Question 3 - Lesson Knowledge
//        // --------------------------------------------------------

//        var importantSentence =
//            FindImportantSentence(
//                sentences,
//                topic);

//        questions.Add(
//            new QuizQuestion
//            {
//                Number = 3,

//                Question =
//                    $"Which statement is true according to the lesson about {topic}?",

//                Options =
//                    ShuffleOptions(
//                        importantSentence,
//                        "The concept should always be ignored.",
//                        "The topic is only useful outside computer science.",
//                        "The topic has no connection to programming."),

//                CorrectAnswer =
//                    importantSentence,

//                Explanation =
//                    "This answer is supported by the provided lesson content."
//            });


//        // --------------------------------------------------------
//        // Question 4 - Understanding
//        // --------------------------------------------------------

//        var understandingSentence =
//            FindAnotherSentence(
//                sentences,
//                definition,
//                importantSentence);

//        questions.Add(
//            new QuizQuestion
//            {
//                Number = 4,

//                Question =
//                    $"Which statement helps explain {topic}?",

//                Options =
//                    ShuffleOptions(
//                        understandingSentence,
//                        "It removes the need for students to practice.",
//                        "It is only used for non-technical subjects.",
//                        "It cannot be applied in software development."),

//                CorrectAnswer =
//                    understandingSentence,

//                Explanation =
//                    "The correct answer comes from the lesson material."
//            });


//        // --------------------------------------------------------
//        // Question 5 - Review
//        // --------------------------------------------------------

//        questions.Add(
//            new QuizQuestion
//            {
//                Number = 5,

//                Question =
//                    $"What should a student do after learning {topic}?",

//                Options =
//                    ShuffleOptions(
//                        "Review the concepts and practice them.",
//                        "Ignore the lesson completely.",
//                        "Memorize unrelated information.",
//                        "Avoid using the concept in practice."),

//                CorrectAnswer =
//                    "Review the concepts and practice them.",

//                Explanation =
//                    "Reviewing and practicing the lesson helps students understand and remember the topic."
//            });


//        return questions
//            .Take(questionCount)
//            .ToList();
//    }


//    // ============================================================
//    // Clean Lesson Content
//    // ============================================================

//    private string CleanLessonContent(
//        string? content)
//    {
//        if (string.IsNullOrWhiteSpace(content))
//        {
//            return string.Empty;
//        }

//        var cleaned = content;

//        // Remove Markdown headings
//        cleaned =
//            Regex.Replace(
//                cleaned,
//                @"^\s*#+\s*",
//                "",
//                RegexOptions.Multiline);

//        // Remove Markdown bold
//        cleaned =
//            cleaned.Replace("**", "");

//        // Remove Markdown italic
//        cleaned =
//            cleaned.Replace("*", "");

//        // Remove code fences
//        cleaned =
//            cleaned.Replace("```", "");

//        // Remove bullet markers
//        cleaned =
//            Regex.Replace(
//                cleaned,
//                @"^\s*[-•]\s*",
//                "",
//                RegexOptions.Multiline);

//        // Remove extra whitespace
//        cleaned =
//            Regex.Replace(
//                cleaned,
//                @"\s+",
//                " ");

//        return cleaned.Trim();
//    }


//    // ============================================================
//    // Extract Sentences
//    // ============================================================

//    private List<string> ExtractSentences(
//        string content)
//    {
//        if (string.IsNullOrWhiteSpace(content))
//        {
//            return new List<string>();
//        }

//        var sentences =
//            Regex.Split(
//                content,
//                @"(?<=[.!?])\s+");

//        return sentences
//            .Select(CleanText)
//            .Where(x =>
//                !string.IsNullOrWhiteSpace(x))
//            .Where(x =>
//                x.Length >= 20)
//            .Where(x =>
//                x.Length <= 250)
//            .Distinct(
//                StringComparer.OrdinalIgnoreCase)
//            .ToList();
//    }


//    // ============================================================
//    // Find Definition
//    // ============================================================

//    private string FindDefinitionSentence(
//        List<string> sentences,
//        string topic)
//    {
//        if (!sentences.Any())
//        {
//            return
//                $"The lesson explains the main concepts of {topic}.";
//        }

//        var definition =
//            sentences.FirstOrDefault(
//                x =>
//                    x.Contains(
//                        " is ",
//                        StringComparison.OrdinalIgnoreCase)
//                    ||
//                    x.Contains(
//                        " are ",
//                        StringComparison.OrdinalIgnoreCase)
//                    ||
//                    x.Contains(
//                        " means ",
//                        StringComparison.OrdinalIgnoreCase));

//        return definition
//            ?? sentences.First();
//    }


//    // ============================================================
//    // Find Important Sentence
//    // ============================================================

//    private string FindImportantSentence(
//        List<string> sentences,
//        string topic)
//    {
//        if (!sentences.Any())
//        {
//            return
//                $"The lesson contains important concepts about {topic}.";
//        }

//        var sentence =
//            sentences.FirstOrDefault(
//                x =>
//                    x.Contains(
//                        topic,
//                        StringComparison.OrdinalIgnoreCase));

//        return sentence
//            ?? sentences.First();
//    }


//    // ============================================================
//    // Find Another Useful Sentence
//    // ============================================================

//    private string FindAnotherSentence(
//        List<string> sentences,
//        string definition,
//        string importantSentence)
//    {
//        var sentence =
//            sentences.FirstOrDefault(
//                x =>
//                    !x.Equals(
//                        definition,
//                        StringComparison.OrdinalIgnoreCase)
//                    &&
//                    !x.Equals(
//                        importantSentence,
//                        StringComparison.OrdinalIgnoreCase));

//        return sentence
//            ?? "The lesson provides useful information about the topic.";
//    }


//    // ============================================================
//    // Clean Text
//    // ============================================================

//    private string CleanText(
//        string? text)
//    {
//        if (string.IsNullOrWhiteSpace(text))
//        {
//            return string.Empty;
//        }

//        var cleaned =
//            text.Trim();

//        cleaned =
//            Regex.Replace(
//                cleaned,
//                @"\s+",
//                " ");

//        return cleaned.Trim();
//    }


//    // ============================================================
//    // Options
//    // ============================================================

//    private List<string> ShuffleOptions(
//        params string[] options)
//    {
//        return options
//            .Where(x =>
//                !string.IsNullOrWhiteSpace(x))
//            .Select(CleanText)
//            .Distinct(
//                StringComparer.OrdinalIgnoreCase)
//            .OrderBy(_ => Random.Shared.Next())
//            .ToList();
//    }
//}

using FlowAISystem.Application.AI.Student.Quiz.Interfaces;
using FlowAISystem.Application.AI.Student.Quiz.Models;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Application.AI.Student.Quiz.Services;

public class QuizGenerator : IQuizGenerator
{
    public Task<QuizResult> GenerateAsync(
        LessonKnowledgeDto lesson,
        int questionCount = 5,
        string difficulty = "Beginner")
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (questionCount <= 0)
            questionCount = 5;

        var content = CleanLessonContent(
            lesson.Content);

        var questions = BuildQuestions(
            lesson.Title,
            content,
            questionCount);

        var result = new QuizResult
        {
            Topic = lesson.Title,
            Difficulty = difficulty,
            Questions = questions
        };

        return Task.FromResult(result);
    }

    private List<QuizQuestion> BuildQuestions(
        string topic,
        string content,
        int questionCount)
    {
        var questions = new List<QuizQuestion>();

        var firstSentence =
            GetFirstSentence(content);

        // Question 1
        questions.Add(new QuizQuestion
        {
            Number = 1,

            Question =
                $"What is the main topic of the lesson \"{topic}\"?",

            Options =
            [
                topic,
                "Computer Networking",
                "Operating Systems",
                "Database Administration"
            ],

            CorrectAnswer = topic,

            Explanation =
                $"The lesson is about {topic}."
        });

        // Question 2
        questions.Add(new QuizQuestion
        {
            Number = 2,

            Question =
                $"Which statement best describes {topic}?",

            Options =
            [
                firstSentence,
                "It is only used for computer hardware.",
                "It is unrelated to programming.",
                "It cannot be used in software."
            ],

            CorrectAnswer = firstSentence,

            Explanation =
                "This answer comes directly from the lesson material."
        });

        // Question 3
        questions.Add(new QuizQuestion
        {
            Number = 3,

            Question =
                $"What is an important point about {topic}?",

            Options =
            [
                firstSentence,
                "It has no practical use.",
                "Students should ignore it.",
                "It is unrelated to computer science."
            ],

            CorrectAnswer = firstSentence,

            Explanation =
                "The lesson identifies this as an important concept."
        });

        // Question 4
        questions.Add(new QuizQuestion
        {
            Number = 4,

            Question =
                $"Why is {topic} useful for students?",

            Options =
            [
                $"It helps students understand {topic}.",
                "It removes the need to study.",
                "It is only useful outside computer science.",
                "It has no practical application."
            ],

            CorrectAnswer =
                $"It helps students understand {topic}.",

            Explanation =
                $"Understanding {topic} helps students learn and apply the concepts in the lesson."
        });

        // Question 5
        questions.Add(new QuizQuestion
        {
            Number = 5,

            Question =
                $"What should students do after learning {topic}?",

            Options =
            [
                "Review the concepts and practice them.",
                "Ignore the lesson.",
                "Study unrelated information.",
                "Avoid practicing the topic."
            ],

            CorrectAnswer =
                "Review the concepts and practice them.",

            Explanation =
                "Reviewing and practicing helps students understand and remember the lesson."
        });

        return questions
            .Take(questionCount)
            .ToList();
    }

    private string CleanLessonContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        var lines = content
            .Split(
                '\n',
                StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Where(x => !x.StartsWith("#"))
            .Where(x => !x.StartsWith("```"))
            .ToList();

        return string.Join(
            " ",
            lines);
    }

    private string GetFirstSentence(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return
                "The lesson explains the main concepts of the topic.";
        }

        var sentence =
            content
                .Split(
                    ['.', '!', '?'],
                    StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(sentence))
        {
            return
                "The lesson explains the main concepts of the topic.";
        }

        return sentence.Trim() + ".";
    }
}