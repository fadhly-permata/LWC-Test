using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repo.Data.Mapping
{
    public partial class CustomerMap
        : IEntityTypeConfiguration<Repo.Data.Entities.Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Repo.Data.Entities.Customer> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("Customer");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INTEGER")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(50)");

            builder.Property(t => t.Hp)
                .IsRequired()
                .HasColumnName("HP")
                .HasColumnType("varchar(15)");

            builder.Property(t => t.Nik)
                .IsRequired()
                .HasColumnName("NIK")
                .HasColumnType("varchar(20)");

            builder.Property(t => t.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("varchar(255)");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "Customer";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string Hp = "HP";
            public const string Nik = "NIK";
            public const string Email = "Email";
        }
        #endregion
    }
}
