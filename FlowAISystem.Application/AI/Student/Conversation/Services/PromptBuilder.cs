using FlowAISystem.Application.AI.Student.Conversation.Interfaces;
using FlowAISystem.Application.AI.Student.Conversation.Models;

namespace FlowAISystem.Application.AI.Student.Conversation.Services;

public class PromptBuilder : IPromptBuilder
{
    // =========================================================
    // Explain More
    // =========================================================

    public string BuildExplainMore(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        The student is continuing a previous conversation.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Explain the previous answer in more depth.

        Requirements:
        - Stay focused on the same topic.
        - Build on the previous answer.
        - Do not simply repeat the previous answer.
        - Add useful details and clarification.
        - Use simple English suitable for a university student.
        - Use an example when it helps understanding.
        - Explain technical terms clearly.
        - Organize the answer with short sections or bullet points when appropriate.
        """;
    }


    // =========================================================
    // Show Example
    // =========================================================

    public string BuildExample(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Provide examples that help the student understand the previous topic.

        Requirements:
        - Stay on the same topic.
        - Start with one simple example.
        - Include one practical or real-world example.
        - Include a programming example when the topic is related to programming.
        - Explain each example clearly.
        - Use simple English.
        - Do not repeat the entire previous explanation.
        """;
    }


    // =========================================================
    // Create Quiz
    // =========================================================

    public string BuildQuiz(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Create a short quiz based on the previous topic.

        Requirements:
        - Create exactly 5 questions.
        - Use multiple-choice questions.
        - Provide 4 choices for each question.
        - Clearly identify the correct answer.
        - Briefly explain why the answer is correct.
        - Test understanding, not memorization only.
        - Keep the difficulty appropriate for a university student.
        - Stay strictly related to the previous topic.
        """;
    }


    // =========================================================
    // Translate
    // =========================================================

    public string BuildTranslation(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous assistant explanation:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Translate the previous explanation into Khmer.

        Requirements:
        - Translate the complete explanation.
        - Preserve the original meaning.
        - Keep technical terms accurate.
        - Do not add unrelated information.
        - Keep important programming and technical terms understandable.
        - Use natural Khmer suitable for a university student.
        """;
    }


    // =========================================================
    // Continue
    // =========================================================

    public string BuildContinue(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Continue the explanation naturally.

        Requirements:
        - Continue from the previous explanation.
        - Do not restart the topic from the beginning.
        - Do not repeat information unnecessarily.
        - Introduce the next useful point or concept.
        - Maintain the same topic.
        - Use simple English.
        - Make the response useful for learning.
        """;
    }


    // =========================================================
    // Compare
    // =========================================================

    public string BuildComparison(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Compare the previous topic with the concept requested by the student.

        Requirements:
        - Identify the two concepts being compared.
        - Explain the similarities.
        - Explain the differences.
        - Use a comparison table when appropriate.
        - Give a short practical example when useful.
        - Avoid unrelated information.
        - Use simple English suitable for a university student.
        """;
    }


    // =========================================================
    // Summary
    // =========================================================

    public string BuildSummary(
        ConversationContext context)
    {
        return $"""
        You are FlowAI, a university learning assistant.

        Previous student question:
        {context.PreviousUserMessage?.Content}

        Previous assistant answer:
        {context.PreviousAssistantMessage?.Content}

        Current student request:
        {context.CurrentQuestion}

        Task:
        Summarize the previous explanation.

        Requirements:
        - Focus only on the important ideas.
        - Use no more than 10 bullet points.
        - Keep technical terms accurate.
        - Remove unnecessary repetition.
        - Make the summary easy for a university student to review.
        - Include important formulas or key rules when relevant.
        """;
    }
}

