using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRestrictionAndDueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "BookIssues");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "BookIssues",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "BookIssues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "BookIssues");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "BookIssues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "BookIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
