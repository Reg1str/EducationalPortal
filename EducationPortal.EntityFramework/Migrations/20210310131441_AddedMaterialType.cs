using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.EntityFramework.Migrations
{
    public partial class AddedMaterialType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Material",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Material");
        }
    }
}
