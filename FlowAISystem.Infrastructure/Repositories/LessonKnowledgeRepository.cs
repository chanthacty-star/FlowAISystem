using Microsoft.EntityFrameworkCore;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Shared.DTOs.AI.LessonKnowledge;

namespace FlowAISystem.Infrastructure.Repositories;

public class LessonKnowledgeRepository : ILessonKnowledgeRepository
{
    private readonly AppDbContext _context;

    public LessonKnowledgeRepository(AppDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // Get All Lessons
    // ==================================================
    public async Task<List<LessonKnowledgeListItemDto>> GetAllAsync(LessonKnowledgeSearchDto search)
    {
        var query = _context.LessonKnowledges
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(x =>
                x.Title.Contains(search.SearchTerm) ||
                x.Content.Contains(search.SearchTerm) ||
                x.Keywords.Contains(search.SearchTerm));
        }

        if (search.TeacherId.HasValue)
        {
            query = query.Where(x => x.TeacherId == search.TeacherId);
        }

        if (search.Difficulty.HasValue)
        {
            query = query.Where(x =>
                x.Difficulty == search.Difficulty.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(x => new LessonKnowledgeListItemDto
            {
                Id = x.Id,
                Title = x.Title,
                TeacherId = x.TeacherId,
                Difficulty = x.Difficulty,
                ActivityType = x.ActivityType,
                Order = x.Order, // lesson index Order
                TeacherName = x.Teacher != null
                    ? x.Teacher.Username
                    : string.Empty,
                //CourseOfferingId = x.CourseOfferingId ?? 0,
                CourseOfferingId = x.CourseOfferingId,
                CourseName = x.CourseOffering != null && x.CourseOffering.Subject != null
                    ? x.CourseOffering.Subject.Name
                    : string.Empty,
                IsActive = x.IsActive,
                KeywordCount = string.IsNullOrEmpty(x.Keywords)
                    ? 0
                    : x.Keywords.Split(',', StringSplitOptions.None).Length,
                ContentLength = x.Content != null ? x.Content.Length : 0,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    // ==================================================
    // Get By Id
    // ==================================================
    public async Task<LessonKnowledgeDto?> GetByIdAsync(int id)
    {
        return await _context.LessonKnowledges
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new LessonKnowledgeDto
            {
                Id = x.Id,

                Title = x.Title,

                Description = x.Description,

                Content = x.Content,

                Difficulty = x.Difficulty,
                ActivityType = x.ActivityType,// for ative sort lesson
                Order = x.Order,

                Keywords = x.Keywords,

                Category = x.Category,


                TeacherId = x.TeacherId,

                TeacherName = x.Teacher != null
                    ? x.Teacher.Username
                    : string.Empty,


                //CourseOfferingId = x.CourseOfferingId ?? 0,
                CourseOfferingId = x.CourseOfferingId,


                CourseName = x.CourseOffering != null &&
                             x.CourseOffering.Subject != null
                    ? x.CourseOffering.Subject.Name
                    : string.Empty,


                ReferenceUrl = x.ReferenceUrl,

                AttachmentPath = x.AttachmentPath,


                IsActive = x.IsActive,


                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    // ==================================================
    // Create
    // ==================================================
    public async Task CreateAsync(CreateLessonKnowledgeDto dto)
    {
        var entity = new LessonKnowledge
        {
            Title = dto.Title,

            Description = dto.Description,

            Content = dto.Content,


            Keywords = dto.Keywords,

            Category = dto.Category,
            Difficulty = dto.Difficulty,
            ActivityType = dto.ActivityType,
            Order = dto.Order,

            TeacherId = dto.TeacherId,

            CourseOfferingId = dto.CourseOfferingId,


            ReferenceUrl = dto.ReferenceUrl,

            AttachmentPath = dto.AttachmentPath,


            IsActive = dto.IsActive,

            CreatedAt = DateTime.UtcNow
        };

        await _context.LessonKnowledges.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // ==================================================
    // Update
    // ==================================================
    public async Task UpdateAsync(UpdateLessonKnowledgeDto dto)
    {
        var entity = await _context.LessonKnowledges
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (entity == null)
            return;

        entity.Title = dto.Title;

        entity.Description = dto.Description;

        entity.Content = dto.Content;


        entity.Keywords = dto.Keywords;

        entity.Category = dto.Category;

        entity.Difficulty = dto.Difficulty;
        entity.ActivityType = dto.ActivityType;
        entity.Order = dto.Order; // Lesson index order
        entity.CourseOfferingId = dto.CourseOfferingId;


        entity.ReferenceUrl = dto.ReferenceUrl;

        entity.AttachmentPath = dto.AttachmentPath;


        entity.IsActive = dto.IsActive;


        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    // ==================================================
    // Delete
    // ==================================================
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.LessonKnowledges.FindAsync(id);

        if (entity == null)
            return;

        _context.LessonKnowledges.Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ==================================================
    // Teacher Lessons
    // ==================================================
    public async Task<List<LessonKnowledgeListItemDto>> GetByTeacherAsync(int teacherId)
    {
        return await GetAllAsync(new LessonKnowledgeSearchDto
        {
            TeacherId = teacherId
        });
    }

    // ==================================================
    // AI Search
    // ==================================================
    public async Task<List<LessonKnowledgeDto>> SearchAsync(
        IEnumerable<string> keywords)
    {
        var searchKeywords = keywords
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct()
            .ToList();

        if (searchKeywords.Count == 0)
        {
            return new List<LessonKnowledgeDto>();
        }

        var query = _context.LessonKnowledges
            .AsNoTracking()
            .Where(x => x.IsActive);

        // Match ANY extracted keyword.
        query = query.Where(x =>
            searchKeywords.Any(keyword =>
                x.Title.Contains(keyword) ||
                x.Description.Contains(keyword) ||
                x.Content.Contains(keyword) ||
                x.Keywords.Contains(keyword) ||
                x.Category.Contains(keyword)));

        return await query
            .Select(x => new LessonKnowledgeDto
            {
                Id = x.Id,

                Title = x.Title,

                Description = x.Description,

                Content = x.Content,

                Keywords = x.Keywords,

                Category = x.Category,

                Difficulty = x.Difficulty,
                ActivityType = x.ActivityType,
                //Order = x.Order,

                CourseOfferingId = x.CourseOfferingId,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }
    // Tutorial lession
    public async Task<List<LessonKnowledgeDto>> GetAllForTutorialAsync(
    LessonKnowledgeSearchDto search)
    {
        var query = _context.LessonKnowledges
            .AsNoTracking()
            .AsQueryable();

        if (search.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == search.IsActive.Value);
        }

        return await query
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Id)
            .Select(x => new LessonKnowledgeDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Content = x.Content,
                Category = x.Category,
                Difficulty = x.Difficulty,
                Order = x.Order,
                IsActive = x.IsActive,
                ActivityType = x.ActivityType
            })
            .ToListAsync();
    }

    // ==================================================
    // AI Lesson
    // ==================================================
    //public async Task<LessonKnowledgeDto?> GetLessonForAIAsync(int lessonId)
    //{
    //    return await GetByIdAsync(lessonId);
    //}
    // ==================================================
    // AI Lesson
    // ==================================================
    public async Task<LessonKnowledgeDto?> GetLessonForAIAsync(int lessonId)
    {
        return await _context.LessonKnowledges
            .AsNoTracking()
            .Where(x => x.Id == lessonId && x.IsActive)
            .Select(x => new LessonKnowledgeDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Content = x.Content,
                Difficulty = x.Difficulty,
                ActivityType = x.ActivityType,
                Order = x.Order,
                Keywords = x.Keywords,
                Category = x.Category,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher != null ? x.Teacher.Username : string.Empty,
                CourseOfferingId = x.CourseOfferingId,
                CourseName = x.CourseOffering != null && x.CourseOffering.Subject != null
                    ? x.CourseOffering.Subject.Name
                    : string.Empty,
                ReferenceUrl = x.ReferenceUrl,
                AttachmentPath = x.AttachmentPath,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
}