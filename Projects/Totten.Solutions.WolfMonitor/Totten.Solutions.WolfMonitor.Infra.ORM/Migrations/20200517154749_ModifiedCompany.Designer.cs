﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations
{
    [DbContext(typeof(WolfMonitorContext))]
    [Migration("20200517154749_ModifiedCompany")]
    partial class ModifiedCompany
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Agents.Agent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CompanyId");

                    b.Property<bool>("Configured");

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<DateTime?>("FirstConnection");

                    b.Property<string>("HostAddress");

                    b.Property<string>("HostName");

                    b.Property<DateTime?>("LastConnection");

                    b.Property<DateTime?>("LastUpload");

                    b.Property<string>("LocalIp");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("MachineName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("Removed");

                    b.Property<DateTime>("UpdatedIn");

                    b.Property<Guid>("UserWhoCreatedId");

                    b.Property<string>("UserWhoCreatedName");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserWhoCreatedId");

                    b.ToTable("Agents");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ff77050f-0f93-41ff-8136-5b8c195a3aa5"),
                            CompanyId = new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                            Configured = false,
                            CreatedIn = new DateTime(2020, 5, 17, 12, 47, 48, 533, DateTimeKind.Local).AddTicks(6647),
                            DisplayName = "Servidor BR 1",
                            Login = "servidor1",
                            Password = "I2uzfR1PyNB3qujyRKe/fvFvXQzylgU+UUIARcpeLkI=",
                            Removed = false,
                            UpdatedIn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserWhoCreatedId = new Guid("f91a2366-c469-412a-9197-976a90516272"),
                            UserWhoCreatedName = "Admin"
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Companies.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Cnae");

                    b.Property<string>("Cnpj")
                        .IsRequired();

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FantasyName")
                        .IsRequired();

                    b.Property<string>("MunicipalRegistration");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<bool>("Removed");

                    b.Property<string>("StateRegistration");

                    b.Property<DateTime>("UpdatedIn");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c576cf93-370c-4464-21f9-08d763d27d75"),
                            Address = "Rua Cicero Lourenço, Mossoró/RN",
                            Cnae = "",
                            Cnpj = "35.344.681/0001-90",
                            CreatedIn = new DateTime(2020, 5, 17, 12, 47, 48, 530, DateTimeKind.Local).AddTicks(5145),
                            Email = "aleffmds@gmail.com",
                            FantasyName = "tottemsolutions",
                            MunicipalRegistration = "",
                            Name = "ALEFF MOURA DA SILVA 10685805425",
                            Phone = "(49) 9 9914-6350",
                            Removed = false,
                            StateRegistration = "",
                            UpdatedIn = new DateTime(2020, 5, 17, 12, 47, 48, 531, DateTimeKind.Local).AddTicks(2180)
                        });
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Agents.Agent", b =>
                {
                    b.HasOne("Totten.Solutions.WolfMonitor.Domain.Features.Companies.Company", "Company")
                        .WithMany("Agents")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
