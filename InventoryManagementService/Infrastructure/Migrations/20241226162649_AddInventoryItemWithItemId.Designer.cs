﻿// <auto-generated />
using InventoryManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryManagementService.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20241226162649_AddInventoryItemWithItemId")]
    partial class AddInventoryItemWithItemId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InventoryManagementService.Domain.Entities.InventoryItem", b =>
                {
                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.ToTable("InventoryItems");
                });
#pragma warning restore 612, 618
        }
    }
}
