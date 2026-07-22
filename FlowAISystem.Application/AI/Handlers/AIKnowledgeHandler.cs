using FlowAISystem.Application.AI.Memory;
using FlowAISystem.Application.AI.Interfaces;
using FlowAISystem.Application.AI.Utilities;
using FlowAISystem.Application.Interfaces.Repositories;

namespace FlowAISystem.Application.AI.Handlers;

public class AIKnowledgeHandler : IAIKnowledgeHandler
{
    private readonly IAIKnowledgeRepository _knowledgeRepository;

    private readonly AIConversationMemory _memory;


    public AIKnowledgeHandler(
        IAIKnowledgeRepository knowledgeRepository,
        AIConversationMemory memory)
    {
        _knowledgeRepository = knowledgeRepository;
        _memory = memory;
    }

    public async Task<string> HandleAsync(
        string question)
    {

        if (string.IsNullOrWhiteSpace(question))
        {
            return "Please enter a question.";
        }

        // ==========================================
        // Follow-up Detection
        // ==========================================

        if (FollowUpDetector.IsFollowUp(question))
        {
            var previousTopic =
                _memory.Get<string>("CurrentKnowledge");

            if (!string.IsNullOrWhiteSpace(previousTopic))
            {
                question =
                    $"{previousTopic} {question}";
            }
        }



        // =====================================
        // Extract Keywords
        // =====================================

        var keywords =
            KeywordExtractor.Extract(question);



        if (keywords.Count == 0)
        {
            return
            "I could not understand your question.";
        }





        // =====================================
        // Top 3 Knowledge Search
        // =====================================

        var matches =
            await _knowledgeRepository
                .SearchTopMatchesAsync(
                    keywords,
                    3);



        if (matches.Count == 0)
        {
            return
            "I couldn't find that information in the FlowAISystem knowledge base.";
        }





        // =====================================
        // Best Match
        // =====================================

        var best =
            matches.First();





        // =====================================
        // Confidence Threshold
        // =====================================

        if (best.Confidence >= 80)
        {

            return
$"""
{best.Answer}


----------------------------------------
Category   : {best.Category}
Confidence : {best.Confidence:F1}%
Score      : {best.Score}/{best.MaxScore}
""";

        }





        // =====================================
        // Medium Confidence
        // =====================================

        if (best.Confidence >= 50)
        {

            return
$"""
I found related information:


Question:
{best.Question}


Answer:
{best.Answer}


----------------------------------------
Category   : {best.Category}
Confidence : {best.Confidence:F1}%
Score      : {best.Score}/{best.MaxScore}
""";

        }





        // =====================================
        // Low Confidence
        // =====================================

        var suggestions =
            string.Join(
                "\n",
                matches.Select(x =>
                    $"• {x.Question} ({x.Confidence:F1}%)"));



        return
$"""
I found some related topics, but I am not confident enough.

Possible topics:

{suggestions}
""";

    }

}