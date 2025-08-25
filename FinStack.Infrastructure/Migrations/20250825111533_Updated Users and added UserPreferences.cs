using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinStack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUsersandaddedUserPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Nationality");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Users",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AspNetUsers",
                newName: "UserType");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "Users",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "AspNetUsers",
                newName: "Type");
        }
    }
}
