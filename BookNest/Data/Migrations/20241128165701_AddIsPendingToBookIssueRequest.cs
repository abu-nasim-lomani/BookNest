using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPendingToBookIssueRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "BookIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "BookIssueRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "BookIssues");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "BookIssueRequests");
        }
    }
}
