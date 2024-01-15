﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace storeAPI.Migrations
{
    public partial class AddProductQtyVar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductQty",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductQty",
                table: "Products");
        }
    }
}
