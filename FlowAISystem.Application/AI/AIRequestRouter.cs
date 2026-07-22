using FlowAISystem.Application.AI.Interfaces;

namespace FlowAISystem.Application.AI;

public class AIRequestRouter
{
    private readonly IAIIntentDetector _intentDetector;

    private readonly IStudentAIHandler _studentHandler;

    private readonly IDashboardAIHandler _dashboardHandler;

    private readonly IAIKnowledgeHandler _knowledgeHandler;

    private readonly IGeneralAIHandler _generalHandler;

    private readonly IReportAIHandler _reportHandler;


    // Constructor
    public AIRequestRouter(
        IAIIntentDetector intentDetector,
        IStudentAIHandler studentHandler,
        IDashboardAIHandler dashboardHandler,
        IAIKnowledgeHandler knowledgeHandler,
        IGeneralAIHandler generalHandler,
        IReportAIHandler reportHandler)
    {
        _intentDetector = intentDetector;
        _studentHandler = studentHandler;
        _dashboardHandler = dashboardHandler;
        _knowledgeHandler = knowledgeHandler;
        _generalHandler = generalHandler;
        _reportHandler = reportHandler;
    }

    public async Task<string> ProcessAsync(
        string question)
    {
        var intent =
            _intentDetector.Detect(question);


        return intent switch
        {
            AIIntent.Student =>
                await _studentHandler.HandleAsync(question),


            AIIntent.Dashboard =>
                await _dashboardHandler.HandleAsync(question),


            AIIntent.AIKnowledge =>
                await _knowledgeHandler.HandleAsync(question),


            AIIntent.Report =>
                await _reportHandler.HandleAsync(question),


            AIIntent.Greeting =>
                await _generalHandler.HandleAsync(question),


            _ =>
                await _generalHandler.HandleAsync(question)
        };
    }
}