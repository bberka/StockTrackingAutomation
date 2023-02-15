using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class namingupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtLogs_Customers_CustomerId",
                table: "DebtLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtLogs_Suppliers_SupplierId",
                table: "DebtLogs");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "DebtLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DebtLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtLogs_Customers_CustomerId",
                table: "DebtLogs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtLogs_Suppliers_SupplierId",
                table: "DebtLogs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtLogs_Customers_CustomerId",
                table: "DebtLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtLogs_Suppliers_SupplierId",
                table: "DebtLogs");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "DebtLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DebtLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtLogs_Customers_CustomerId",
                table: "DebtLogs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtLogs_Suppliers_SupplierId",
                table: "DebtLogs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
