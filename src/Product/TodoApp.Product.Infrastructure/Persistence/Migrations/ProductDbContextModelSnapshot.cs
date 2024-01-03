﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TodoApp.Product.Infrastructure.Persistence;

#nullable disable

namespace TodoApp.Product.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    partial class ProductDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TodoApp.Product.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e6b60336-4de2-447c-bfa7-c1ca684d4bdc"),
                            CreateAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Product 1",
                            Price = 9.99m,
                            Quantity = 10
                        },
                        new
                        {
                            Id = new Guid("96a55738-c9e8-4856-84ec-a3eaeca28c5f"),
                            CreateAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Product 2",
                            Price = 19.99m,
                            Quantity = 5
                        },
                        new
                        {
                            Id = new Guid("22e7cd5e-609d-462c-a3e0-43907c703243"),
                            CreateAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Product 3",
                            Price = 14.99m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = new Guid("8c75831e-dee9-48a1-84fb-82047897dcba"),
                            CreateAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Product 4",
                            Price = 7.99m,
                            Quantity = 8
                        },
                        new
                        {
                            Id = new Guid("d4e89984-e367-4018-95fa-3aa1ed6caf80"),
                            CreateAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Product 5",
                            Price = 24.99m,
                            Quantity = 2
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
