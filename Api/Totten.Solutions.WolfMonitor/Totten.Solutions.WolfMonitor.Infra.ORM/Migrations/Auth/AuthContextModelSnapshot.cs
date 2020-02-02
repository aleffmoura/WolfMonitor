﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.Auth
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedIn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedIn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6a5ecc23-e4ea-4077-ac04-6836e188ab0b"),
                            CreatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 704, DateTimeKind.Local).AddTicks(4752),
                            Level = 0,
                            Name = "Agent",
                            Removed = false,
                            UpdatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 705, DateTimeKind.Local).AddTicks(4502)
                        },
                        new
                        {
                            Id = new Guid("527d5006-c3bf-4d97-8c7d-387a967894fe"),
                            CreatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8287),
                            Level = 1,
                            Name = "User",
                            Removed = false,
                            UpdatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8306)
                        },
                        new
                        {
                            Id = new Guid("f91a2366-c469-412a-9197-976a90516272"),
                            CreatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8447),
                            Level = 2,
                            Name = "Admin",
                            Removed = false,
                            UpdatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8449)
                        },
                        new
                        {
                            Id = new Guid("93409973-4e84-44f8-8997-b452921dba34"),
                            CreatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8539),
                            Level = 3,
                            Name = "System",
                            Removed = false,
                            UpdatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 706, DateTimeKind.Local).AddTicks(8541)
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedIn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ccc8b552-9b77-4318-88ba-56beeea131fe"),
                            CompanyId = new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                            Cpf = "10685805425",
                            CreatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(193),
                            Email = "aleffmds@gmail.com",
                            FirstName = "Aleff",
                            Language = "pt-BR",
                            LastName = "Moura da Silva",
                            Login = "aleffmoura",
                            Password = "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=",
                            Removed = false,
                            RoleId = new Guid("93409973-4e84-44f8-8997-b452921dba34"),
                            UpdatedIn = new DateTime(2020, 2, 2, 12, 54, 2, 723, DateTimeKind.Local).AddTicks(230)
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation.User", b =>
                {
                    b.HasOne("Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
