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

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(x => new LessonKnowledgeListItemDto
            {
                Id = x.Id,
                Title = x.Title,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher != null
                    ? x.Teacher.Username
                    : string.Empty,
                CourseOfferingId = x.CourseOfferingId ?? 0,
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
                Description = null,
                Content = x.Content,
                Keywords = x.Keywords,
                Category = string.Empty,
                TeacherId = x.TeacherId,
                TeacherName = x.Teacher != null
                    ? x.Teacher.Username
                    : string.Empty,
                CourseOfferingId = x.CourseOfferingId ?? 0,
                CourseName = x.CourseOffering != null && x.CourseOffering.Subject != null
                    ? x.CourseOffering.Subject.Name
                    : string.Empty,
                ReferenceUrl = null,
                AttachmentPath = null,
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
            Content = dto.Content,
            Keywords = dto.Keywords,
            TeacherId = dto.TeacherId,
            CourseOfferingId = dto.CourseOfferingId,
            IsActive = true,
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
        entity.Content = dto.Content;
        entity.Keywords = dto.Keywords;
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
    public async Task<List<LessonKnowledgeDto>> SearchAsync(string keyword)
    {
        return await _context.LessonKnowledges
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                (
                    x.Title.Contains(keyword) ||
                    x.Content.Contains(keyword) ||
                    x.Keywords.Contains(keyword)
                ))
            .Select(x => new LessonKnowledgeDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Keywords = x.Keywords,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    // ==================================================
    // AI Lesson
    // ==================================================
    public async Task<LessonKnowledgeDto?> GetLessonForAIAsync(int lessonId)
    {
        return await GetByIdAsync(lessonId);
    }
}