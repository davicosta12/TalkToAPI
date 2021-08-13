using Microsoft.EntityFrameworkCore.Migrations;

namespace TalkToApi.Migrations
{
    public partial class Atualization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_AspNetUsers_DeId",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_AspNetUsers_ParaId",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_AspNetUsers_UsuarioId",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Tokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Tokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Mensagem",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParaId",
                table: "Mensagem",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeId",
                table: "Mensagem",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_AspNetUsers_DeId",
                table: "Mensagem",
                column: "DeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_AspNetUsers_ParaId",
                table: "Mensagem",
                column: "ParaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_AspNetUsers_UsuarioId",
                table: "Tokens",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_AspNetUsers_DeId",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_AspNetUsers_ParaId",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_AspNetUsers_UsuarioId",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Tokens",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Tokens",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Mensagem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ParaId",
                table: "Mensagem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "DeId",
                table: "Mensagem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_AspNetUsers_DeId",
                table: "Mensagem",
                column: "DeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_AspNetUsers_ParaId",
                table: "Mensagem",
                column: "ParaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_AspNetUsers_UsuarioId",
                table: "Tokens",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
