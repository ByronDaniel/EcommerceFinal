using BP.Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Infraestructure.EntitiesConfigurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property("Id")
                         .ValueGeneratedOnAdd()
                         .HasColumnType("uniqueidentifier");

            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("nvarchar(30)");

            builder.HasKey("Id");

            builder.ToTable("Brands");

        }
    }
}
