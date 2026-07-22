using FlowAISystem.Shared.DTOs.Feedbacks;

namespace FlowAISystem.Application.Interfaces.Services;

public interface IFeedbackService
{

    Task<List<FeedbackListItemDto>> GetAllAsync(
        FeedbackSearchDto search);



    Task<FeedbackDto?> GetByIdAsync(
        int id);



    Task CreateAsync(
        CreateFeedbackDto dto);



    Task UpdateAsync(
        UpdateFeedbackDto dto);



    Task DeleteAsync(
        int id);

}