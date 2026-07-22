namespace FlowAISystem.Shared.DTOs.AI;

public class AIKnowledgeRankingDto
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


    public double Confidence
    {
        get
        {
            if (MaxScore == 0)
                return 0;


            var value =
                (double)Score / MaxScore * 100;


            return Math.Round(
                Math.Min(value, 100),
                2);
        }
    }
}