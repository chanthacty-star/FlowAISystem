namespace FlowAISystem.Application.AI.Utilities;

public static class FollowUpDetector
{
    private static readonly HashSet<string> FollowUpWords =
    [
        "it",
        "its",
        "this",
        "that",
        "they",
        "them",
        "he",
        "she",
        "him",
        "her",
        "more",
        "again",
        "continue",
        "why",
        "how",
        "when",
        "where",
        "who",
        "which"
    ];

    public static bool IsFollowUp(
        string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return false;
        }

        question = question.ToLower();

        return FollowUpWords.Any(
            word => question.Contains(word));
    }
}