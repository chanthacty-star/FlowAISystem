using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.AI;
using FlowAISystem.Shared.DTOs.Reports;


namespace FlowAISystem.Application.Interfaces.Repositories;


public interface IAIKnowledgeRepository
{

    // =====================================
    // AI SEARCH
    // =====================================

    Task<List<AIKnowledgeRankingDto>>
        SearchTopMatchesAsync(
            List<string> keywords,
            int limit = 3);



    Task<AIKnowledge?>
        SearchAsync(
            string keyword);



    // =====================================
    // CRUD
    // =====================================

    Task<List<AIKnowledge>>
        GetAllAsync();



    Task<AIKnowledge?>
        GetByIdAsync(
            int id);



    Task AddAsync(
        AIKnowledge knowledge);



    Task UpdateAsync(
        AIKnowledge knowledge);



    Task DeleteAsync(
        int id);



    // =====================================
    // REPORT
    // =====================================

    Task<List<AIKnowledgeReportDto>>
        GetAIKnowledgeReportAsync();

}