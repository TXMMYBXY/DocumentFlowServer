using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableToDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_CreatedBy",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Documents");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_CreatedBy",
                table: "Documents",
                newName: "IX_Documents_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_CreatedBy",
                table: "Documents",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_CreatedBy",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "Contracts");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CreatedBy",
                table: "Contracts",
                newName: "IX_Contracts_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_CreatedBy",
                table: "Contracts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
