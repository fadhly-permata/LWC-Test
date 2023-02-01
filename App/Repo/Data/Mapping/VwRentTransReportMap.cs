using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Mapping
{
    public partial class VwRentTransReportMap
        : IEntityTypeConfiguration<Repo.Data.Entities.VwRentTransReport>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Repo.Data.Entities.VwRentTransReport> builder)
        {
            #region Generated Configure
            // table
            builder.ToView("VW_RENT_TRANS_REPORT");

            // key
            builder.HasNoKey();

            // properties
            builder.Property(t => t.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("INTEGER");

            builder.Property(t => t.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.TotalRentTrans)
                .HasColumnName("TotalRentTrans")
                .HasColumnType("INTEGER");

            builder.Property(t => t.TotalActiveRent)
                .HasColumnName("TotalActiveRent")
                .HasColumnType("INTEGER");

            builder.Property(t => t.TotalReturned)
                .HasColumnName("TotalReturned")
                .HasColumnType("INTEGER");

            builder.Property(t => t.TotalPaidFine)
                .HasColumnName("TotalPaidFine")
                .HasColumnType("INTEGER");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "VW_RENT_TRANS_REPORT";
        }

        public struct Columns
        {
            public const string CustomerId = "CustomerId";
            public const string Name = "Name";
            public const string TotalRentTrans = "TotalRentTrans";
            public const string TotalActiveRent = "TotalActiveRent";
            public const string TotalReturned = "TotalReturned";
            public const string TotalPaidFine = "TotalPaidFine";
        }
        #endregion
    }
}
