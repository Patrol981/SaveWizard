using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveWizard.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRepoNameDateToBackup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Backups",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RepositoryName",
                table: "Backups",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Backups");

            migrationBuilder.DropColumn(
                name: "RepositoryName",
                table: "Backups");
        }
    }
}
