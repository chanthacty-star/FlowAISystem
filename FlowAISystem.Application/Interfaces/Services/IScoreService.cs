using FlowAISystem.Shared.DTOs.Scores;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IScoreService
{

    Task<List<ScoreListItemDto>> GetAllAsync(
        ScoreSearchDto search);



    Task<ScoreDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateScoreDto dto);



    Task UpdateAsync(
        UpdateScoreDto dto);



    Task DeleteAsync(
        int id);

}