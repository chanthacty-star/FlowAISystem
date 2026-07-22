//namespace FlowAISystem.Shared.DTOs.AI;

//public class AIKnowledgeMatchResultDto
//{
//    public int KnowledgeId { get; set; }

//    public string Question { get; set; } = string.Empty;

//    public string Answer { get; set; } = string.Empty;

//    public int Score { get; set; }

//    public int MaxScore { get; set; }

//    public double Confidence =>
//        MaxScore == 0
//            ? 0
//            : Math.Round((double)Score / MaxScore * 100, 2);
//}

namespace FlowAISystem.Shared.DTOs.AI;

public class AIKnowledgeMatchResultDto
{
    public int KnowledgeId { get; set; }


    public string Question { get; set; }
        = string.Empty;


    public string Answer { get; set; }
        = string.Empty;


    public string Category { get; set; }
        = string.Empty;


    public int Score { get; set; }


    public int MaxScore { get; set; }


    public double Confidence =>
        MaxScore == 0
            ? 0
            : Math.Round(
                (double)Score / MaxScore * 100,
                2);


    public DateTime CreatedAt { get; set; }


    public List<string> MatchedKeywords { get; set; }
        = new();
}