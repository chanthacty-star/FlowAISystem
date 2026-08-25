using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;

namespace FlowAISystem.Application.AI.Student.Services;

public class StudentIntentDetector
    : IStudentIntentDetector
{
    public StudentIntent Detect(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return StudentIntent.Unknown;
        }

        message = message
            .ToLowerInvariant()
            .Trim();


        // =====================================================
        // Greeting
        // =====================================================

        if (
            message.Contains("hello") ||
            message.Contains("hi") ||
            message.Contains("hey") ||
            message.Contains("good morning") ||
            message.Contains("good afternoon") ||

            message.Contains("សួស្តី") ||
            message.Contains("ជំរាបសួរ") ||
            message.Contains("អរុណសួស្តី")
        )
        {
            return StudentIntent.Greeting;
        }


        // =====================================================
        // Thank You
        // =====================================================

        if (
            message.Contains("thank") ||
            message.Contains("thanks") ||
            message.Contains("appreciate") ||

            message.Contains("អរគុណ") ||
            message.Contains("អរគុណច្រើន")
        )
        {
            return StudentIntent.ThankYou;
        }


        // =====================================================
        // Conversation / Follow-up
        // IMPORTANT:
        // These must be checked BEFORE Quiz.
        // =====================================================

        if (
            message.Contains("explain more") ||
            message.Contains("explain again") ||
            message.Contains("more detail") ||
            message.Contains("show example") ||
            message.Contains("give example") ||
            message.Contains("create quiz") ||
            message.Contains("make a quiz") ||
            message.Contains("continue") ||
            message.Contains("compare") ||
            message.Contains("summarize") ||
            message.Contains("summary") ||
            message.Contains("translate") ||

            message.Contains("ពន្យល់បន្ថែម") ||
            message.Contains("ពន្យល់ម្ដងទៀត") ||
            message.Contains("ឧទាហរណ៍") ||
            message.Contains("ធ្វើតេស្ត") ||
            message.Contains("បន្ត") ||
            message.Contains("ប្រៀបធៀប") ||
            message.Contains("សង្ខេប") ||
            message.Contains("បកប្រែ")
        )
        {
            return StudentIntent.Conversation;
        }


        // =====================================================
        // Standalone Quiz Request
        // =====================================================

        if (
            message.Contains("quiz") ||
            message.Contains("test me") ||
            message.Contains("practice question") ||
            message.Contains("give me questions") ||

            message.Contains("សំណួរ") ||
            message.Contains("ប្រឡង")
        )
        {
            return StudentIntent.Quiz;
        }


        // =====================================================
        // Recommendation
        // =====================================================

        if (
            message.Contains("recommend") ||
            message.Contains("what should i study") ||
            message.Contains("what should i learn") ||
            message.Contains("next topic") ||

            message.Contains("តើខ្ញុំគួររៀនអ្វី") ||
            message.Contains("គួររៀនអ្វីបន្ទាប់") ||
            message.Contains("ណែនាំ")
        )
        {
            return StudentIntent.Recommendation;
        }


        // =====================================================
        // Academic Information
        // =====================================================

        if (
            message.Contains("gpa") ||
            message.Contains("grade") ||
            message.Contains("score") ||
            message.Contains("attendance") ||
            message.Contains("subject") ||
            message.Contains("schedule") ||

            message.Contains("ពិន្ទុ") ||
            message.Contains("និទ្ទេស") ||
            message.Contains("វត្តមាន") ||
            message.Contains("មុខវិជ្ជា") ||
            message.Contains("កាលវិភាគ")
        )
        {
            return StudentIntent.AcademicInformation;
        }


        // =====================================================
        // Learning
        // =====================================================

        if (
            message.Contains("explain") ||
            message.Contains("what is") ||
            message.Contains("what are") ||
            message.Contains("tell me about") ||
            message.Contains("how does") ||
            message.Contains("define") ||
            message.Contains("meaning") ||

            message.Contains("ពន្យល់") ||
            message.Contains("អ្វីជា") ||
            message.Contains("មានន័យថា") ||
            message.Contains("ប្រាប់ខ្ញុំអំពី") ||
            message.Contains("តើអ្វី")
        )
        {
            return StudentIntent.Learn;
        }


        // =====================================================
        // General Conversation
        // =====================================================

        return StudentIntent.General;
    }
}

