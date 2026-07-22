using FlowAISystem.Domain.Common;

namespace FlowAISystem.Domain.Entities;

public class TrainingData : BaseEntity
{
    public string InputData { get; set; } = string.Empty;

    public string ExpectedOutput { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public bool IsUsedForTraining { get; set; }
}