using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAndPhoneToEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Tenants_TenantId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TenantId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId",
                table: "Employees",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Tenants_TenantId",
                table: "Employees",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
