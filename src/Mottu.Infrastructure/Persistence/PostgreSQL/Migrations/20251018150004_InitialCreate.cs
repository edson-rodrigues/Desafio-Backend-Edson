using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cnh_number = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    cnh_type = table.Column<string>(type: "text", nullable: false),
                    cnh_image_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_drivers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    license_plate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    motorcycle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expected_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actual_return_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    plan_duration_days = table.Column<int>(type: "integer", nullable: false),
                    plan_daily_cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    plan_penalty_percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    total_cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_delivery_drivers_delivery_driver_id",
                        column: x => x.delivery_driver_id,
                        principalTable: "delivery_drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rentals_motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_delivery_drivers_cnpj",
                table: "delivery_drivers",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_delivery_drivers_identifier",
                table: "delivery_drivers",
                column: "identifier");

            migrationBuilder.CreateIndex(
                name: "ix_motorcycles_identifier",
                table: "motorcycles",
                column: "identifier");

            migrationBuilder.CreateIndex(
                name: "ix_motorcycles_license_plate",
                table: "motorcycles",
                column: "license_plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rentals_delivery_driver_id",
                table: "rentals",
                column: "delivery_driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_rentals_motorcycle_id",
                table: "rentals",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "ix_rentals_status",
                table: "rentals",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "delivery_drivers");

            migrationBuilder.DropTable(
                name: "motorcycles");
        }
    }
}
