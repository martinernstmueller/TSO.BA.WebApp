using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace TSO.BA.WebApp.Migrations.Floor
{
    public partial class FloorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    FloorId = table.Column<Guid>(nullable: false),
                    FloorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.FloorId);
                });
            migrationBuilder.CreateTable(
                name: "BuildingState",
                columns: table => new
                {
                    BuildingStateId = table.Column<Guid>(nullable: false),
                    BuildingStateKey = table.Column<string>(nullable: true),
                    BuildingStateValue = table.Column<string>(nullable: true),
                    FloorFloorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingState", x => x.BuildingStateId);
                    table.ForeignKey(
                        name: "FK_BuildingState_Floor_FloorFloorId",
                        column: x => x.FloorFloorId,
                        principalTable: "Floor",
                        principalColumn: "FloorId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("BuildingState");
            migrationBuilder.DropTable("Floor");
        }
    }
}
