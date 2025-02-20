using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.SaleId);

        builder.Property(s => s.SaleId)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.Customer)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Total)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(s => s.Branch)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Products)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Quantities)
            .IsRequired();

        builder.Property(s => s.UnitPrices)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(s => s.Discounts)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(s => s.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(s => s.IsCancelled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(s => s.SaleCreated)
            .IsRequired();

        builder.Property(s => s.SaleModified)
            .IsRequired(false);

        builder.Property(s => s.SaleCancelled)
            .IsRequired(false);

        builder.Property(s => s.ItemCancelled)
            .IsRequired()
            .HasDefaultValue(false);
    }
}