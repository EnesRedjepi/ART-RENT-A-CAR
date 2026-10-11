using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextileShop.Domain.Entities;

namespace TextileShop.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(order => order.OrderNumber).IsRequired().HasMaxLength(30);
        builder.Property(order => order.Notes).HasMaxLength(500);
        builder.Property(order => order.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(order => order.PaymentStatus).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(order => order.OrderNumber).IsUnique();
        builder.HasIndex(order => order.OrderDate);

        builder.HasOne(order => order.Customer)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(order => order.ShippingAddress)
            .WithMany()
            .HasForeignKey(order => order.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(item => item.ProductName).IsRequired().HasMaxLength(150);
        builder.Property(item => item.ProductSku).IsRequired().HasMaxLength(50);
        builder.Ignore(item => item.TotalPrice);

        builder.HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
