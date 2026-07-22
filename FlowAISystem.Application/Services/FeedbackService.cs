using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Feedbacks;

namespace FlowAISystem.Application.Services;

public class FeedbackService : IFeedbackService
{

    private readonly IFeedbackRepository _repository;


    public FeedbackService(
        IFeedbackRepository repository)
    {
        _repository = repository;
    }





    public async Task<List<FeedbackListItemDto>> GetAllAsync(
        FeedbackSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }






    public async Task<FeedbackDto?> GetByIdAsync(
        int id)
    {

        var feedback =
            await _repository.GetByIdAsync(id);



        if (feedback == null)
        {
            return null;
        }



        return new FeedbackDto
        {

            Id = feedback.Id,


            Rating = feedback.Rating,


            Comment = feedback.Comment,


            FeedbackDate =
                feedback.FeedbackDate,


            EnrollmentId =
                feedback.EnrollmentId,


            StudentName =
                feedback.Enrollment!
                .Student!
                .Name
                .ToString(),


            CourseName =
                feedback.Enrollment!
                .CourseOffering!
                .Subject!
                .Name,


            TeacherName =
                feedback.Enrollment!
                .CourseOffering!
                .Teacher!
                .TeacherName,


            SemesterName =
                feedback.Enrollment!
                .CourseOffering!
                .Semester!
                .Name

        };

    }







    public async Task CreateAsync(
        CreateFeedbackDto dto)
    {

        var exists =
            await _repository.ExistsAsync(
                dto.EnrollmentId);



        if (exists)
        {
            throw new Exception(
                "Feedback already exists for this enrollment.");
        }



        var feedback = new Feedback
        {

            EnrollmentId =
                dto.EnrollmentId,


            Rating =
                dto.Rating,


            Comment =
                dto.Comment,


            FeedbackDate =
                dto.FeedbackDate

        };



        await _repository.CreateAsync(feedback);

    }







    public async Task UpdateAsync(
        UpdateFeedbackDto dto)
    {

        var feedback =
            await _repository.GetByIdAsync(dto.Id);



        if (feedback == null)
        {
            throw new Exception(
                "Feedback not found.");
        }



        feedback.Rating =
            dto.Rating;


        feedback.Comment =
            dto.Comment;


        feedback.FeedbackDate =
            dto.FeedbackDate;



        await _repository.UpdateAsync(feedback);

    }







    public async Task DeleteAsync(
        int id)
    {

        var feedback =
            await _repository.GetByIdAsync(id);



        if (feedback == null)
        {
            throw new Exception(
                "Feedback not found.");
        }



        await _repository.DeleteAsync(feedback);

    }


}