using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ChangedAssessmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_CardScores_CardId",
                table: "CardScores");

            migrationBuilder.AddColumn<int>(
                name: "Assessment",
                table: "CardScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CardScores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CardScores_CardId",
                table: "CardScores",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardScores_CardId",
                table: "CardScores");

            migrationBuilder.DropColumn(
                name: "Assessment",
                table: "CardScores");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CardScores");

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardScoreId = table.Column<long>(type: "bigint", nullable: true),
                    Score = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_CardScores_CardScoreId",
                        column: x => x.CardScoreId,
                        principalTable: "CardScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardScores_CardId",
                table: "CardScores",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CardScoreId",
                table: "Assessments",
                column: "CardScoreId");
        }
    }
}
