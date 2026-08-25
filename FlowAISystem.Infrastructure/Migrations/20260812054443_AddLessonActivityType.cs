using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonActivityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityType",
                table: "LessonKnowledges",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityType",
                table: "LessonKnowledges");
        }
    }
}
