﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Totten.Solutions.WolfMonitor.Infra.ORM.Contexts;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Migrations.WolfMonitoring
{
    [DbContext(typeof(WolfMonitoringContext))]
    [Migration("20200610225256_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutCurrentValue")
                        .IsRequired();

                    b.Property<Guid>("AgentId");

                    b.Property<Guid>("CompanyId");

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("Default");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("LastValue");

                    b.Property<DateTime?>("MonitoredAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("Removed");

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdatedIn");

                    b.Property<Guid>("UserIdWhoAdd");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserIdWhoAdd");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.ItemHistoric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedIn");

                    b.Property<Guid>("ItemId");

                    b.Property<string>("MonitoredAt");

                    b.Property<bool>("Removed");

                    b.Property<DateTime>("UpdatedIn");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Historic");
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.ItemSolicitationHistoric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AgentId");

                    b.Property<Guid>("CompanyId");

                    b.Property<DateTime>("CreatedIn");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<Guid>("ItemId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("Removed");

                    b.Property<int>("SolicitationType");

                    b.Property<DateTime>("UpdatedIn");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("SolicitationsHistoric");
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.ItemHistoric", b =>
                {
                    b.HasOne("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.Item")
                        .WithMany("Historic")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.ItemSolicitationHistoric", b =>
                {
                    b.HasOne("Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation.Item")
                        .WithMany("SolicitationHistoric")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
