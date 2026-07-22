using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Scores;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IScoreRepository
{

    Task<List<ScoreListItemDto>> GetAllAsync(
        ScoreSearchDto search);



    Task<Score?> GetByIdAsync(
        int id);



    Task CreateAsync(
        Score score);



    Task UpdateAsync(
        Score score);



    Task DeleteAsync(
        Score score);



    Task<bool> ExistsAsync(
        int enrollmentId,
        string assessmentName,
        int? ignoreId = null);

}