using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphQLProject.Migrations
{
    /// <inheritdoc />
    public partial class GraphqlContext02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Actv_Ind",
                table: "menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Crtd_Dt",
                table: "menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Crtd_User",
                table: "menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Del_Ind",
                table: "menu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Lst_Crtd_Dt",
                table: "menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lst_Crtd_User",
                table: "menu",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actv_Ind",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Crtd_Dt",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Crtd_User",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Del_Ind",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Lst_Crtd_Dt",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "Lst_Crtd_User",
                table: "menu");
        }
    }
}
