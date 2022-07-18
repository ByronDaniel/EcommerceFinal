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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

            builder.Property("BrandId")
                .HasColumnType("uniqueidentifier");

            builder.Property("Description")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property("Name")
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnType("nvarchar(40)");

            builder.Property("Price")
                .HasColumnType("decimal(18,2)");

            builder.Property("ProductTypeId")
                .HasColumnType("uniqueidentifier");

            builder.Property("Stock")
                .HasColumnType("int");

            builder.HasKey("Id");
            builder.HasIndex("BrandId");
            builder.HasIndex("ProductTypeId");

            builder.ToTable("Products");

            builder.HasOne(p => p.Brand)
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
            builder.Navigation("Brand");

            builder.HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey("ProductTypeId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Navigation("ProductType");

        }
    }
}
