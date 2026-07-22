using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Application.Interfaces.Services;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.Scores;

namespace FlowAISystem.Application.Services;

public class ScoreService : IScoreService
{

    private readonly IScoreRepository _repository;


    public ScoreService(
        IScoreRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<ScoreListItemDto>> GetAllAsync(
        ScoreSearchDto search)
    {
        return await _repository.GetAllAsync(search);
    }



    public async Task<ScoreDto?> GetByIdAsync(
        int id)
    {

        var score =
            await _repository.GetByIdAsync(id);


        if (score == null)
            return null;



        return new ScoreDto
        {

            Id = score.Id,

            AssessmentName =
                score.AssessmentName,


            Marks =
                score.Marks,


            MaxMarks =
                score.MaxMarks,


            AssessmentDate =
                score.AssessmentDate,


            EnrollmentId =
                score.EnrollmentId,


            StudentName =
                score.Enrollment?.Student?.Name.ToString()
                ?? string.Empty,


            CourseName =
                score.Enrollment?
                .CourseOffering?
                .Subject?
                .Name
                ?? string.Empty

        };

    }



    public async Task CreateAsync(
        CreateScoreDto dto)
    {

        var score = new Score
        {

            EnrollmentId =
                dto.EnrollmentId,


            AssessmentName =
                dto.AssessmentName,


            Marks =
                dto.Marks,


            MaxMarks =
                dto.MaxMarks,


            AssessmentDate =
                dto.AssessmentDate

        };


        await _repository.CreateAsync(score);

    }



    public async Task UpdateAsync(
        UpdateScoreDto dto)
    {

        var score =
            await _repository.GetByIdAsync(dto.Id);



        if (score == null)
            throw new Exception(
                "Score not found");



        score.EnrollmentId =
            dto.EnrollmentId;


        score.AssessmentName =
            dto.AssessmentName;


        score.Marks =
            dto.Marks;


        score.MaxMarks =
            dto.MaxMarks;


        score.AssessmentDate =
            dto.AssessmentDate;



        await _repository.UpdateAsync(score);

    }



    public async Task DeleteAsync(
        int id)
    {

        var score =
            await _repository.GetByIdAsync(id);



        if (score == null)
            throw new Exception(
                "Score not found");



        await _repository.DeleteAsync(score);

    }

}