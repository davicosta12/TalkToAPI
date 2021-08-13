using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TalkToApi.Migrations
{
    public partial class MensagemExcluidaAtualizada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Atualizado",
                table: "Mensagem",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Mensagem",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atualizado",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Mensagem");

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
