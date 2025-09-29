using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_picpay.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "display_name",
                table: "accounts",
                type: "varchar",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "public_id",
                table: "accounts",
                type: "varchar",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ux_accounts_display_name",
                table: "accounts",
                column: "display_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_accounts_public_id",
                table: "accounts",
                column: "public_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_accounts_display_name",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ux_accounts_public_id",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "display_name",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "public_id",
                table: "accounts");
        }
    }
}
