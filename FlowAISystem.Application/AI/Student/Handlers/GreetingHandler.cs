using FlowAISystem.Application.AI.Student.Enums;
using FlowAISystem.Application.AI.Student.Interfaces;
using FlowAISystem.Shared.DTOs.AI;

namespace FlowAISystem.Application.AI.Student.Handlers;


public class GreetingHandler: IStudentAIWorkflowHandler
{

    public StudentIntent Intent
        => StudentIntent.Greeting;



    public Task<StudentAIResponseDto> HandleAsync(
        StudentAIRequestDto request)
    {

        var isKhmer =
            request.Language?
                .StartsWith(
                    "km",
                    StringComparison.OrdinalIgnoreCase)
            == true;



        return Task.FromResult(
            new StudentAIResponseDto
            {
                Answer = isKhmer
                    ?
                    "សួស្តី 👋 ខ្ញុំជា FlowAI Learning Assistant។ តើខ្ញុំអាចជួយអ្នកអ្វីបានខ្លះ?"
                    :
                    "Hello 👋 I am FlowAI Learning Assistant. How can I help you today?",


                Source =
                    "Student AI",


                Confidence =
                    1.0m,


                CreatedAt =
                    DateTime.UtcNow
            });
    }
}