using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLProject.Migrations
{
    /// <inheritdoc />
    public partial class GraphqlContext03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "menu",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Crtd_User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Crtd_Dt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lst_Crtd_User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Lst_Crtd_Dt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Actv_Ind = table.Column<int>(type: "int", nullable: false),
                    Del_Ind = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartySize = table.Column<int>(type: "int", nullable: false),
                    SpecialRequest = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Crtd_User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Crtd_Dt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lst_Crtd_User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Lst_Crtd_Dt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Actv_Ind = table.Column<int>(type: "int", nullable: false),
                    Del_Ind = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_menu_CategoryId",
                table: "menu",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_menu_Category_CategoryId",
                table: "menu",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menu_Category_CategoryId",
                table: "menu");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_menu_CategoryId",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "menu");
        }
    }
}
