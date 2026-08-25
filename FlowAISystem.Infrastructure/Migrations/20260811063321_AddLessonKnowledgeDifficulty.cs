using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonKnowledgeDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonKnowledges_CourseOfferings_CourseOfferingId",
                table: "LessonKnowledges");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonKnowledges_Users_TeacherId",
                table: "LessonKnowledges");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LessonKnowledges",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceUrl",
                table: "LessonKnowledges",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "LessonKnowledges",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "LessonKnowledges",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LessonKnowledges",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "LessonKnowledges",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentPath",
                table: "LessonKnowledges",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "LessonKnowledges",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonKnowledges_CourseOfferings_CourseOfferingId",
                table: "LessonKnowledges",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonKnowledges_Users_TeacherId",
                table: "LessonKnowledges",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonKnowledges_CourseOfferings_CourseOfferingId",
                table: "LessonKnowledges");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonKnowledges_Users_TeacherId",
                table: "LessonKnowledges");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "LessonKnowledges");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceUrl",
                table: "LessonKnowledges",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "LessonKnowledges",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "LessonKnowledges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentPath",
                table: "LessonKnowledges",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonKnowledges_CourseOfferings_CourseOfferingId",
                table: "LessonKnowledges",
                column: "CourseOfferingId",
                principalTable: "CourseOfferings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonKnowledges_Users_TeacherId",
                table: "LessonKnowledges",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
