using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loginDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSavedToUserRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "userRate",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "userRate");
        }
    }
}
