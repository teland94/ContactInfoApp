using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactInfoApp.Server.Migrations
{
    public partial class SearchContactHistoryComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchContactHistoryComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorImage = table.Column<string>(type: "TEXT", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    Liked = table.Column<int>(type: "INTEGER", nullable: false),
                    Disliked = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SearchContactHistoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchContactHistoryComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchContactHistoryComments_SearchContactHistory_SearchContactHistoryId",
                        column: x => x.SearchContactHistoryId,
                        principalTable: "SearchContactHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchContactHistoryComments_SearchContactHistoryId",
                table: "SearchContactHistoryComments",
                column: "SearchContactHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchContactHistoryComments");
        }
    }
}
