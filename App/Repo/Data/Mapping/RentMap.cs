using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Mapping
{
    public partial class RentMap
        : IEntityTypeConfiguration<Repo.Data.Entities.Rent>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Repo.Data.Entities.Rent> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("Rent");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INTEGER")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("INTEGER");

            builder.Property(t => t.LockerId)
                .HasColumnName("LockerId")
                .HasColumnType("INTEGER");

            builder.Property(t => t.RentDate)
                .HasColumnName("RentDate")
                .HasColumnType("TEXT");

            builder.Property(t => t.ReturnDate)
                .HasColumnName("ReturnDate")
                .HasColumnType("TEXT");

            builder.Property(t => t.Password)
                .HasColumnName("Password")
                .HasColumnType("TEXT");

            builder.Property(t => t.Status)
                .HasColumnName("Status")
                .HasColumnType("INTEGER")
                .HasDefaultValueSql("0");

            builder.Property(t => t.TotalFine)
                .HasColumnName("TotalFine")
                .HasColumnType("INTEGER")
                .HasDefaultValueSql("0");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "Rent";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string CustomerId = "CustomerId";
            public const string LockerId = "LockerId";
            public const string RentDate = "RentDate";
            public const string ReturnDate = "ReturnDate";
            public const string Password = "Password";
            public const string Status = "Status";
            public const string TotalFine = "TotalFine";
        }
        #endregion
    }
}
