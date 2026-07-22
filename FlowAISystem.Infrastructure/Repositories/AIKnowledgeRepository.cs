using FlowAISystem.Shared.DTOs.AI;
using FlowAISystem.Shared.DTOs.Reports;
using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace FlowAISystem.Infrastructure.Repositories;


public class AIKnowledgeRepository : IAIKnowledgeRepository
{

    private readonly AppDbContext _context;



    public AIKnowledgeRepository(
        AppDbContext context)
    {
        _context = context;
    }



    // =====================================================
    // TOP 3 KNOWLEDGE RANKING
    // =====================================================

    public async Task<List<AIKnowledgeRankingDto>>
        SearchTopMatchesAsync(
            List<string> keywords,
            int limit = 3)
    {

        var knowledgeList =
            await _context.AIKnowledge
                .Include(x => x.Category)
                .ToListAsync();



        var results =
            new List<AIKnowledgeRankingDto>();



        foreach (var knowledge in knowledgeList)
        {

            int score = 0;

            int keywordMatches = 0;



            var question =
                knowledge.Question
                .ToLower();



            var keywordText =
                knowledge.Keywords
                .ToLower();



            var answer =
                knowledge.Answer
                .ToLower();



            foreach (var item in keywords)
            {

                var keyword =
                    item.ToLower();



                // Question match
                // strongest
                if (question.Contains(keyword))
                {
                    score += 5;

                    keywordMatches++;
                }



                // Keyword match

                if (keywordText.Contains(keyword))
                {
                    score += 3;

                    keywordMatches++;
                }



                // Answer match

                if (answer.Contains(keyword))
                {
                    score += 1;
                }

            }



            // Multiple keyword bonus

            if (keywordMatches >= 2)
            {
                score += 2;
            }



            // Exact phrase bonus

            var phrase =
                string.Join(
                    " ",
                    keywords);



            if (question.Contains(phrase))
            {
                score += 3;
            }



            if (score > 0)
            {

                int maxScore =
                    (keywords.Count * 9) + 5;



                results.Add(
                    new AIKnowledgeRankingDto
                    {

                        KnowledgeId =
                            knowledge.Id,


                        Question =
                            knowledge.Question,


                        Answer =
                            knowledge.Answer,


                        Category =
                            knowledge.Category.Name,


                        Score =
                            score,


                        MaxScore =
                            maxScore

                    });

            }

        }



        return results

            .OrderByDescending(
                x => x.Confidence)

            .Take(limit)

            .ToList();

    }





    // =====================================================
    // SIMPLE SEARCH
    // =====================================================

    public async Task<AIKnowledge?> SearchAsync(
        string keyword)
    {

        keyword =
            keyword.ToLower();



        return await _context.AIKnowledge

            .FirstOrDefaultAsync(x =>

                x.Keywords
                    .ToLower()
                    .Contains(keyword)

                ||

                x.Question
                    .ToLower()
                    .Contains(keyword)

            );

    }





    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<List<AIKnowledge>>
        GetAllAsync()
    {

        return await _context.AIKnowledge

            .OrderByDescending(
                x => x.CreatedAt)

            .ToListAsync();

    }





    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<AIKnowledge?>
        GetByIdAsync(
            int id)
    {

        return await _context.AIKnowledge

            .FirstOrDefaultAsync(
                x => x.Id == id);

    }





    // =====================================================
    // ADD
    // =====================================================

    public async Task AddAsync(
        AIKnowledge knowledge)
    {

        await _context.AIKnowledge
            .AddAsync(knowledge);



        await _context.SaveChangesAsync();

    }





    // =====================================================
    // UPDATE
    // =====================================================

    public async Task UpdateAsync(
        AIKnowledge knowledge)
    {

        _context.AIKnowledge
            .Update(knowledge);



        await _context.SaveChangesAsync();

    }





    // =====================================================
    // DELETE
    // =====================================================

    public async Task DeleteAsync(
        int id)
    {

        var knowledge =
            await GetByIdAsync(id);



        if (knowledge != null)
        {

            _context.AIKnowledge
                .Remove(knowledge);



            await _context.SaveChangesAsync();

        }

    }





    // =====================================================
    // REPORT
    // =====================================================

    public async Task<List<AIKnowledgeReportDto>>
        GetAIKnowledgeReportAsync()
    {

        return await _context.AIKnowledge

            .Include(x => x.Category)

            .OrderByDescending(
                x => x.CreatedAt)

            .Select(x => new AIKnowledgeReportDto
            {

                Question =
                    x.Question,


                Category =
                    x.Category.Name,


                CreatedAt =
                    x.CreatedAt


            })

            .ToListAsync();

    }

}
