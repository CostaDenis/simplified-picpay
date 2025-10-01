using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_picpay.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document",
                table: "accounts",
                newName: "document");

            migrationBuilder.AlterColumn<string>(
                name: "document",
                table: "accounts",
                type: "varchar",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ux_accounts_document",
                table: "accounts",
                column: "document",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_accounts_document",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "document",
                table: "accounts",
                newName: "Document");

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 14);
        }
    }
}
