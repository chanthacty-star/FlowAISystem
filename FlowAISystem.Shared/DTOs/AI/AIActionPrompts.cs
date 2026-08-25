namespace FlowAISystem.Shared.DTOs.AI;

public static class AIActionPrompts
{
    public static readonly Dictionary<string, string> FollowUps =
        new()
        {
            {
                "Explain More",
                "Explain the previous answer in more detail."
            },

            {
                "Show Example",
                "Show a practical example."
            },

            {
                "Create Quiz",
                "Create a quiz from this topic."
            },

            {
                "Translate",
                "Translate the explanation to Khmer."
            }
        };
}