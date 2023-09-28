using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odin.Auth.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    document = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    street_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    street_number = table.Column<int>(type: "integer", nullable: true),
                    complement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    neighborhood = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    zip_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    state = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_updated_by = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
