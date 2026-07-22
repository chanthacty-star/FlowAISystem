using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class Prediction : BaseEntity
{
    public string PredictionType { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public decimal ConfidenceScore { get; set; }


    public int StudentId { get; set; }

    public Student? Student { get; set; }
}