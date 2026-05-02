using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractTableToUniversalDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractTemplates_Id",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Contracts",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreatedBy",
                table: "Contracts",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_CreatedBy",
                table: "Contracts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_CreatedBy",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CreatedBy",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Contracts",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractTemplates_Id",
                table: "Contracts",
                column: "Id",
                principalTable: "ContractTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
