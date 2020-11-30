using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSG_API.Migrations
{
    public partial class DaminChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Servico",
                keyColumn: "Id",
                keyValue: new Guid("55f55cda-42fe-4f3e-8753-b1ac0b73b0e0"));

            migrationBuilder.DeleteData(
                table: "Servico",
                keyColumn: "Id",
                keyValue: new Guid("ea0704ab-c58c-41cd-ac71-3b59f2e171b2"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("62e05296-1513-4105-8b96-06a9494c91f5"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("c789afba-514a-489c-897c-2b3530a9d26b"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("e52a72a7-0cf4-4d63-9ff9-1defddadc1a6"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPrestacao",
                table: "OrdemDeServico",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Servico",
                columns: new[] { "Id", "DescricaoServico", "Nome" },
                values: new object[,]
                {
                    { new Guid("8cba7429-3e11-4601-a724-15ab293e564b"), "Ótimo pintor, especialista em desenhos e pinturas artísticas.", "PINTOR" },
                    { new Guid("a5f55959-6d6b-42fe-a74e-2e5280eae028"), "Especialista em encanamentos e no conserto de vazamentos em geral..", "ENCANADOR" }
                });

            migrationBuilder.InsertData(
                table: "UnidadedeCobranca",
                columns: new[] { "Id", "Unidade" },
                values: new object[,]
                {
                    { new Guid("a80f9265-4751-4cad-ac27-09e62ff4a342"), "Unidade" },
                    { new Guid("2635b394-906d-4376-9e40-44f1e4d5884e"), "Dia" },
                    { new Guid("95949a93-372e-422b-9973-62ad3327a5c6"), "Hora" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Servico",
                keyColumn: "Id",
                keyValue: new Guid("8cba7429-3e11-4601-a724-15ab293e564b"));

            migrationBuilder.DeleteData(
                table: "Servico",
                keyColumn: "Id",
                keyValue: new Guid("a5f55959-6d6b-42fe-a74e-2e5280eae028"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("2635b394-906d-4376-9e40-44f1e4d5884e"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("95949a93-372e-422b-9973-62ad3327a5c6"));

            migrationBuilder.DeleteData(
                table: "UnidadedeCobranca",
                keyColumn: "Id",
                keyValue: new Guid("a80f9265-4751-4cad-ac27-09e62ff4a342"));

            migrationBuilder.AlterColumn<string>(
                name: "DataPrestacao",
                table: "OrdemDeServico",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.InsertData(
                table: "Servico",
                columns: new[] { "Id", "DescricaoServico", "Nome" },
                values: new object[,]
                {
                    { new Guid("ea0704ab-c58c-41cd-ac71-3b59f2e171b2"), "Ótimo pintor, especialista em desenhos e pinturas artísticas.", "PINTOR" },
                    { new Guid("55f55cda-42fe-4f3e-8753-b1ac0b73b0e0"), "Especialista em encanamentos e no conserto de vazamentos em geral..", "ENCANADOR" }
                });

            migrationBuilder.InsertData(
                table: "UnidadedeCobranca",
                columns: new[] { "Id", "Unidade" },
                values: new object[,]
                {
                    { new Guid("62e05296-1513-4105-8b96-06a9494c91f5"), "Unidade" },
                    { new Guid("e52a72a7-0cf4-4d63-9ff9-1defddadc1a6"), "Dia" },
                    { new Guid("c789afba-514a-489c-897c-2b3530a9d26b"), "Hora" }
                });
        }
    }
}
