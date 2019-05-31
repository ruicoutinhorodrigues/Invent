﻿// <auto-generated />
using System;
using Invent.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Invent.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190511222402_ChangeInventoryVM")]
    partial class ChangeInventoryVM
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Invent.Web.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Inventory");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Manufacturer");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime>("EntryDate");

                    b.Property<string>("ImageUrl");

                    b.Property<int>("InventoryId");

                    b.Property<DateTime>("LastChangeDate");

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductModelId");

                    b.Property<string>("ReferenceCode")
                        .IsRequired();

                    b.Property<int>("StatusId");

                    b.Property<int>("SupplierId");

                    b.Property<string>("UserId");

                    b.Property<decimal?>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("InventoryId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductModelId");

                    b.HasIndex("StatusId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("UserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.ProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ManufacturerId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("ProductModel");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("NIPC")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("PostCode")
                        .IsRequired();

                    b.Property<string>("Town");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CloseDate");

                    b.Property<string>("GuyName");

                    b.Property<int?>("InventoryId");

                    b.Property<DateTime>("OpenDate");

                    b.Property<string>("ProdReferenceCode")
                        .IsRequired();

                    b.Property<string>("Remedy")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Trouble")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Product", b =>
                {
                    b.HasOne("Invent.Web.Data.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Invent.Web.Data.Entities.Inventory", "Inventory")
                        .WithMany("Products")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Invent.Web.Data.Entities.Location", "Location")
                        .WithMany("Products")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Invent.Web.Data.Entities.ProductModel", "ProductModel")
                        .WithMany("Products")
                        .HasForeignKey("ProductModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Invent.Web.Data.Entities.Status", "Status")
                        .WithMany("Products")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Invent.Web.Data.Entities.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.ProductModel", b =>
                {
                    b.HasOne("Invent.Web.Data.Entities.Manufacturer", "Manufacturer")
                        .WithMany("ProductModelss")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Invent.Web.Data.Entities.Ticket", b =>
                {
                    b.HasOne("Invent.Web.Data.Entities.Inventory", "Inventory")
                        .WithMany("Tickets")
                        .HasForeignKey("InventoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
