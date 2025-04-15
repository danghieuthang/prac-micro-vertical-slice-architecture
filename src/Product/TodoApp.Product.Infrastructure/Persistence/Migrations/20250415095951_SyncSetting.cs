using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoApp.Product.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22e7cd5e-609d-462c-a3e0-43907c703243"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8c75831e-dee9-48a1-84fb-82047897dcba"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("96a55738-c9e8-4856-84ec-a3eaeca28c5f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4e89984-e367-4018-95fa-3aa1ed6caf80"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e6b60336-4de2-447c-bfa7-c1ca684d4bdc"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreateAt", "ModifiedAt", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("22e7cd5e-609d-462c-a3e0-43907c703243"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Product 3", 14.99m, 3 },
                    { new Guid("8c75831e-dee9-48a1-84fb-82047897dcba"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Product 4", 7.99m, 8 },
                    { new Guid("96a55738-c9e8-4856-84ec-a3eaeca28c5f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Product 2", 19.99m, 5 },
                    { new Guid("d4e89984-e367-4018-95fa-3aa1ed6caf80"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Product 5", 24.99m, 2 },
                    { new Guid("e6b60336-4de2-447c-bfa7-c1ca684d4bdc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Product 1", 9.99m, 10 }
                });
        }
    }
}
