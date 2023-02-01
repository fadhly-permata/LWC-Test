using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Mapping
{
    public partial class LockerMap
        : IEntityTypeConfiguration<Repo.Data.Entities.Locker>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Repo.Data.Entities.Locker> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("Locker");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INTEGER")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Number)
                .IsRequired()
                .HasColumnName("Number")
                .HasColumnType("varchar(2)");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "Locker";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string Number = "Number";
        }
        #endregion
    }
}
