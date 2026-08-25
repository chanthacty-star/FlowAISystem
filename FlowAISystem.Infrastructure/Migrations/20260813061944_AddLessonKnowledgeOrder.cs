using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonKnowledgeOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "LessonKnowledges",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "LessonKnowledges");
        }
    }
}
