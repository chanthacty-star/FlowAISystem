using Markdig;
using FlowAISystem.Application.AI.Interfaces;

namespace FlowAISystem.Application.AI.Services;

public class MarkdownService : IMarkdownService
{

    private readonly MarkdownPipeline pipeline;


    public MarkdownService()
    {

        pipeline =
            new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

    }



    public string ConvertToHtml(string markdown)
    {

        return Markdown.ToHtml(
            markdown,
            pipeline
        );

    }

}