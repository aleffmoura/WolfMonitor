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
    [Migration("20191108150111_Modified-Company-AddColmuns-CreatedIn-UpdatedIn")]
    partial class ModifiedCompanyAddColmunsCreatedInUpdatedIn
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

                    b.Property<DateTime?>("FirstConnection");

                    b.Property<string>("HostAddress");

                    b.Property<string>("HostName");

                    b.Property<DateTime?>("LastConnection");

                    b.Property<DateTime?>("LastUpload");

                    b.Property<string>("LocalIp");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("Removed");

                    b.Property<DateTime>("UpdatedIn");

                    b.Property<Guid>("UserWhoCreated");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.Companies.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cnpj")
                        .IsRequired();

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("FantasyName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Removed");

                    b.Property<DateTime>("UpdatedIn");

                    b.HasKey("Id");

                    b.ToTable("Companies");
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