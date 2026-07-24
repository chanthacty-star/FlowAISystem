namespace FlowAISystem.Application.AI.Student.Interfaces;

/// <summary>
/// Extracts important keywords or topics from a student's message.
/// These keywords are used for searching lessons, courses,
/// and academic knowledge.
/// </summary>
public interface IKeywordExtractor
{
    /// <summary>
    /// Extracts meaningful keywords from a message.
    /// </summary>
    
    /// Student input message.
    /// </param>
    /// <returns>
    /// A collection of extracted keywords.
    /// </returns>
    IEnumerable<string> Extract(string message);
}