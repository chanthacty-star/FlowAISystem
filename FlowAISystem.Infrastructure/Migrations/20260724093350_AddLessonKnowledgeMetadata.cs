using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonKnowledgeMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "LessonKnowledges",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceUrl",
                table: "LessonKnowledges",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "LessonKnowledges");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "LessonKnowledges");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LessonKnowledges");

            migrationBuilder.DropColumn(
                name: "ReferenceUrl",
                table: "LessonKnowledges");
        }
    }
}
