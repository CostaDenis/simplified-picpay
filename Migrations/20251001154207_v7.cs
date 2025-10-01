using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_picpay.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "payee_public_id",
                table: "transactions",
                type: "varchar",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payer_public_id",
                table: "transactions",
                type: "varchar",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payee_public_id",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "payer_public_id",
                table: "transactions");
        }
    }
}
