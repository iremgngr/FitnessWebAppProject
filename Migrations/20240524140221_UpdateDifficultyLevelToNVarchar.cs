using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loginDemo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDifficultyLevelToNVarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_city",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_city", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_todo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    category = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    difficultyLevel = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_todo__3213E83F716950B9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userDetail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    biography = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDetail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userRate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    todoId = table.Column<int>(type: "int", nullable: true),
                    rate = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRate", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_city");

            migrationBuilder.DropTable(
                name: "tbl_todo");

            migrationBuilder.DropTable(
                name: "userDetail");

            migrationBuilder.DropTable(
                name: "userRate");
        }
    }
}
