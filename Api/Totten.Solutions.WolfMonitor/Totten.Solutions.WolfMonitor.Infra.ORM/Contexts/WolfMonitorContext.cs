using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Infra.ORM.Contexts
{
    public class WolfMonitorContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }

        public WolfMonitorContext(DbContextOptions<WolfMonitorContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PatientEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new AttendanceEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new ExamResultEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new SymptomAttendanceEntityConfiguration());

            //modelBuilder.ApplyConfiguration(new ExamEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new SymptomEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new TreatmentEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new MedicineEntityConfiguration());


            //modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
    }
}
