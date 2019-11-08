﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
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
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Users.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedIn");

                    b.Property<int>("Level");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Removed");

                    b.Property<DateTime>("UpdatedIn");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("71b178bf-e972-4c55-837e-ec9d3e45806f"),
                            CreatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 207, DateTimeKind.Local).AddTicks(4775),
                            Level = 0,
                            Name = "Agent",
                            Removed = false,
                            UpdatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 213, DateTimeKind.Local).AddTicks(2463)
                        },
                        new
                        {
                            Id = new Guid("599a14e9-02c1-47ae-9046-8cf3771a7044"),
                            CreatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8631),
                            Level = 1,
                            Name = "User",
                            Removed = false,
                            UpdatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8690)
                        },
                        new
                        {
                            Id = new Guid("6d936a6b-1102-4838-8ef9-c452e6451861"),
                            CreatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8827),
                            Level = 2,
                            Name = "Administrador",
                            Removed = false,
                            UpdatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8846)
                        },
                        new
                        {
                            Id = new Guid("93c434db-c88a-41cc-be95-aa57838bc8ec"),
                            CreatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8895),
                            Level = 2,
                            Name = "System",
                            Removed = false,
                            UpdatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 216, DateTimeKind.Local).AddTicks(8910)
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompanyId");

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<bool>("Removed");

                    b.Property<Guid>("RoleId");

                    b.Property<DateTime>("UpdatedIn");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("403f5ecd-5f98-434e-8a01-3109e7908b76"),
                            CompanyId = new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                            Cpf = "10685805425",
                            CreatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 254, DateTimeKind.Local).AddTicks(4386),
                            Email = "aleffmds@gmail.com",
                            FirstName = "Aleff",
                            Language = "pt-BR",
                            LastName = "Moura da Silva",
                            Login = "alefmoura",
                            Password = "YWLA/fjq/N2i5CmP6+HJMXDslpQwLLP2tHl4E9NaM8w=",
                            Removed = false,
                            RoleId = new Guid("93c434db-c88a-41cc-be95-aa57838bc8ec"),
                            UpdatedIn = new DateTime(2019, 11, 8, 17, 56, 5, 254, DateTimeKind.Local).AddTicks(4450)
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Users.User", b =>
                {
                    b.HasOne("Totten.Solutions.WolfMonitor.Domain.Features.Users.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}