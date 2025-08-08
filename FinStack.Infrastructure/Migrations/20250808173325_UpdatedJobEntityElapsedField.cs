using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinStack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedJobEntityElapsedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ElapsedMs",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ElapsedMs",
                table: "Jobs",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
