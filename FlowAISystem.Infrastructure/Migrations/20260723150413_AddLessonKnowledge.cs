using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonKnowledge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonKnowledges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Keywords = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    CourseOfferingId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonKnowledges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonKnowledges_CourseOfferings_CourseOfferingId",
                        column: x => x.CourseOfferingId,
                        principalTable: "CourseOfferings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonKnowledges_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonKnowledges_CourseOfferingId",
                table: "LessonKnowledges",
                column: "CourseOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonKnowledges_TeacherId",
                table: "LessonKnowledges",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonKnowledges");
        }
    }
}
