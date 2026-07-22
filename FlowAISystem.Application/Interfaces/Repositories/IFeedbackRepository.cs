using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Feedbacks;

namespace FlowAISystem.Application.Interfaces.Repositories;

public interface IFeedbackRepository
{

    Task<List<FeedbackListItemDto>> GetAllAsync(
        FeedbackSearchDto search);



    Task<Feedback?> GetByIdAsync(
        int id);



    Task CreateAsync(
        Feedback feedback);



    Task UpdateAsync(
        Feedback feedback);



    Task DeleteAsync(
        Feedback feedback);



    Task<bool> ExistsAsync(
        int enrollmentId,
        int? ignoreId = null);

}