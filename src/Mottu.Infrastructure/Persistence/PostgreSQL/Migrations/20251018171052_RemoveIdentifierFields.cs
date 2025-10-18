using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentifierFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_motorcycles_identifier",
                table: "motorcycles");

            migrationBuilder.DropIndex(
                name: "ix_delivery_drivers_identifier",
                table: "delivery_drivers");

            migrationBuilder.DropColumn(
                name: "identifier",
                table: "motorcycles");

            migrationBuilder.DropColumn(
                name: "identifier",
                table: "delivery_drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identifier",
                table: "motorcycles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "identifier",
                table: "delivery_drivers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_motorcycles_identifier",
                table: "motorcycles",
                column: "identifier");

            migrationBuilder.CreateIndex(
                name: "ix_delivery_drivers_identifier",
                table: "delivery_drivers",
                column: "identifier");
        }
    }
}
