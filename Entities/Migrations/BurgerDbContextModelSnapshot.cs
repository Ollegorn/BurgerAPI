﻿// <auto-generated />
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(BurgerDbContext))]
    partial class BurgerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Burger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal");

                    b.HasKey("Id");

                    b.ToTable("Burgers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ingredients = "Beef,Cheese,Tomato",
                            Name = "Cheeseburger",
                            Price = 12m
                        },
                        new
                        {
                            Id = 2,
                            Ingredients = "Beef,Cheese,Bacon,Caramelized_Onion,Mushroom",
                            Name = "Special",
                            Price = 15m
                        },
                        new
                        {
                            Id = 3,
                            Ingredients = "\"Beef,Cheese,Tomato,Egg,Letucce\"",
                            Name = "Egger",
                            Price = 13m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}