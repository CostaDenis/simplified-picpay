using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace simplified_picpay.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
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
                    email = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "varchar", maxLength: 256, nullable: false),
                    current_balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "storekeepers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    cnpj = table.Column<string>(type: "varchar", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storekeepers", x => x.id);
                    table.ForeignKey(
                        name: "fk_storekeepers_account_id",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    cpf = table.Column<string>(type: "varchar", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_account_id",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_accounts_email",
                table: "accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_storekeepers_AccountId",
                table: "storekeepers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_storekeepers_cnpj",
                table: "storekeepers",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_AccountId",
                table: "users",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_users_cpf",
                table: "users",
                column: "cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "storekeepers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
