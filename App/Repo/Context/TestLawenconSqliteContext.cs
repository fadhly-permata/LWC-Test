using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Repo.Context
{
    public partial class TestLawenconSqliteContext : DbContext
    {
        public TestLawenconSqliteContext(DbContextOptions<TestLawenconSqliteContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            #pragma warning disable CS1030 // #warning directive
            => optionsBuilder.UseSqlite("Data Source=/home/permata/Documents/DB/test-lawencon.sqlite.db");

        #region Generated Properties
        public virtual DbSet<Repo.Data.Entities.Customer> Customers { get; set; } = null!;

        public virtual DbSet<Repo.Data.Entities.EmailLog> EmailLogs { get; set; } = null!;

        public virtual DbSet<Repo.Data.Entities.Locker> Lockers { get; set; } = null!;

        public virtual DbSet<Repo.Data.Entities.Rent> Rents { get; set; } = null!;

        public virtual DbSet<Repo.Data.Entities.VwRentTransReport> VwRentTransReports { get; set; } = null!;

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Generated Configuration
            modelBuilder.ApplyConfiguration(new Repo.Data.Mapping.CustomerMap());
            modelBuilder.ApplyConfiguration(new Repo.Data.Mapping.EmailLogMap());
            modelBuilder.ApplyConfiguration(new Repo.Data.Mapping.LockerMap());
            modelBuilder.ApplyConfiguration(new Repo.Data.Mapping.RentMap());
            modelBuilder.ApplyConfiguration(new Repo.Data.Mapping.VwRentTransReportMap());
            #endregion

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
        }
    }
}
