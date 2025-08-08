using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinStack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedJobEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Jobs",
                newName: "StartTime");

            migrationBuilder.AddColumn<double>(
                name: "ElapsedMs",
                table: "Jobs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishTime",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Jobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Jobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "Jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElapsedMs",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Jobs",
                newName: "CreatedDate");
        }
    }
}
