using FlowAISystem.Application.AI.Student.Enums;

namespace FlowAISystem.Application.AI.Student.Interfaces;

/// <summary>
/// Detects the primary intent of a student's message.
/// This helps the Student AI decide how the request
/// should be processed.
/// </summary>
public interface IStudentIntentDetector
{
    /// <summary>
    /// Analyzes the student's message and determines its intent.
    /// </summary>
    /// <param 
  
    StudentIntent Detect(string message);
}