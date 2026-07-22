using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowAISystem.Infrastructure.Migrations
{
    public partial class AddAICategoryRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create AICategories table first
            migrationBuilder.CreateTable(
                name: "AICategories",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    Name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false),

                    Description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false),

                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_AICategories",
                        x => x.Id);
                });


            // 2. Add CategoryId temporarily nullable
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AIKnowledge",
                type: "integer",
                nullable: true);


            // 3. Create default categories from old values
            migrationBuilder.Sql(
            """
            INSERT INTO "AICategories"
            ("Name", "Description", "CreatedAt")
            VALUES
            ('Programming', 'Programming knowledge', NOW()),
            ('Database', 'Database knowledge', NOW()),
            ('AI', 'Artificial Intelligence knowledge', NOW()),
            ('Security', 'Security knowledge', NOW());
            """);


            // 4. Move old Category data into CategoryId
            migrationBuilder.Sql(
            """
            UPDATE "AIKnowledge"
            SET "CategoryId" =
            CASE
                WHEN "Category" = 'Programming' THEN 1
                WHEN "Category" = 'Database' THEN 2
                WHEN "Category" = 'AI' THEN 3
                WHEN "Category" = 'Security' THEN 4
                ELSE 1
            END;
            """);


            // 5. Make CategoryId required
            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "AIKnowledge",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);


            // 6. Create index
            migrationBuilder.CreateIndex(
                name: "IX_AIKnowledge_CategoryId",
                table: "AIKnowledge",
                column: "CategoryId");


            // 7. Create FK relationship
            migrationBuilder.AddForeignKey(
                name: "FK_AIKnowledge_AICategories_CategoryId",
                table: "AIKnowledge",
                column: "CategoryId",
                principalTable: "AICategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            // 8. Remove old Category column
            migrationBuilder.DropColumn(
                name: "Category",
                table: "AIKnowledge");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore old Category column
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AIKnowledge",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");


            migrationBuilder.DropForeignKey(
                name: "FK_AIKnowledge_AICategories_CategoryId",
                table: "AIKnowledge");


            migrationBuilder.DropIndex(
                name: "IX_AIKnowledge_CategoryId",
                table: "AIKnowledge");


            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AIKnowledge");


            migrationBuilder.DropTable(
                name: "AICategories");
        }
    }
}