using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TSO.BA.WebApp.Models;

namespace TSO.BA.WebApp.Migrations.Floor
{
    [DbContext(typeof(FloorContext))]
    [Migration("20160315215029_FloorMigration")]
    partial class FloorMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TSO.BA.WebApp.Models.BuildingState", b =>
                {
                    b.Property<Guid>("BuildingStateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuildingStateKey");

                    b.Property<string>("BuildingStateValue");

                    b.Property<Guid?>("FloorFloorId");

                    b.HasKey("BuildingStateId");
                });

            modelBuilder.Entity("TSO.BA.WebApp.Models.Floor", b =>
                {
                    b.Property<Guid>("FloorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FloorName");

                    b.HasKey("FloorId");
                });

            modelBuilder.Entity("TSO.BA.WebApp.Models.BuildingState", b =>
                {
                    b.HasOne("TSO.BA.WebApp.Models.Floor")
                        .WithMany()
                        .HasForeignKey("FloorFloorId");
                });
        }
    }
}
