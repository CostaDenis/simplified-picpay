using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_picpay.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "varchar", maxLength: 30, nullable: false),
                    public_id = table.Column<string>(type: "varchar", maxLength: 36, nullable: false),
                    email = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "varchar", maxLength: 256, nullable: false),
                    current_balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    account_type = table.Column<string>(type: "varchar", maxLength: 11, nullable: false),
                    document = table.Column<string>(type: "varchar", maxLength: 14, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    payer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payer_public_id = table.Column<string>(type: "varchar", maxLength: 36, nullable: false),
                    payee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payee_public_id = table.Column<string>(type: "varchar", maxLength: 36, nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_payee_id",
                        column: x => x.payee_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_payer_id",
                        column: x => x.payer_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ux_accounts_display_name",
                table: "accounts",
                column: "display_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_accounts_document",
                table: "accounts",
                column: "document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_accounts_email",
                table: "accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_accounts_public_id",
                table: "accounts",
                column: "public_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_payee_id",
                table: "transactions",
                column: "payee_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_payer_id",
                table: "transactions",
                column: "payer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
