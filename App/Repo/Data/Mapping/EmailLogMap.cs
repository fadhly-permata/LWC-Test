using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Mapping
{
    public partial class EmailLogMap
        : IEntityTypeConfiguration<Repo.Data.Entities.EmailLog>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Repo.Data.Entities.EmailLog> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("EmailLog");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INTEGER")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.CustomerId)
                .IsRequired()
                .HasColumnName("CustomerId")
                .HasColumnType("INTEGER");

            builder.Property(t => t.LockerId)
                .IsRequired()
                .HasColumnName("LockerId")
                .HasColumnType("INTEGER");

            builder.Property(t => t.Message)
                .IsRequired()
                .HasColumnName("Message")
                .HasColumnType("TEXT");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnName("Status")
                .HasColumnType("INTEGER");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "EmailLog";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string CustomerId = "CustomerId";
            public const string LockerId = "LockerId";
            public const string Message = "Message";
            public const string Status = "Status";
        }
        #endregion
    }
}
