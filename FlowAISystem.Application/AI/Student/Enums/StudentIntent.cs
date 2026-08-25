namespace FlowAISystem.Application.AI.Student.Enums;

/// <summary>
/// Represents the student's intention when interacting
/// with the Student AI Learning Assistant.
/// </summary>
public enum StudentIntent
{
    /// <summary>
    /// Unable to determine the student's intent.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// General greetings or polite conversations.
    /// Examples:
    /// "Hello"
    /// "Hi"
    /// "Good morning"
    /// </summary>
    Greeting,
    ThankYou,   // new

    /// <summary>
    /// Learn or understand an academic topic.
    /// Examples:
    /// "Explain Binary Search"
    /// "What is OOP?"
    /// "Tell me about Database Normalization"
    /// </summary>
    Learn,

    /// <summary>
    /// Access the student's personal academic information.
    /// Examples:
    /// "Show my GPA"
    /// "My attendance"
    /// "My grades"
    /// "My subjects"
    /// </summary>
    AcademicInformation,

    /// <summary>
    /// Request a quiz or practice questions.
    /// Examples:
    /// "Quiz me"
    /// "Give me five questions"
    /// "Test my knowledge"
    /// </summary>
    Quiz,

    /// <summary>
    /// Ask for learning recommendations.
    /// Examples:
    /// "What should I study next?"
    /// "Recommend related lessons"
    /// </summary>
    Recommendation,

    /// <summary>
    /// Continue an existing conversation.
    /// Examples:
    /// "Give me an example."
    /// "Explain it again."
    /// "Compare them."
    /// </summary>
    Conversation,

    /// <summary>
    /// General questions that don't fit another category.
    /// </summary>
    /// 
    CodeHelp,          // new
    Summarize,         // new
    Translate,         // new
    GeneratePractice,  // new

    General
}