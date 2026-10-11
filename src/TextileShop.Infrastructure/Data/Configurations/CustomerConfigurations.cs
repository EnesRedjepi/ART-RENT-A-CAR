using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextileShop.Domain.Entities;

namespace TextileShop.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FullName).IsRequired().HasMaxLength(100);
    }
}

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(address => address.RecipientName).IsRequired().HasMaxLength(100);
        builder.Property(address => address.PhoneNumber).IsRequired().HasMaxLength(30);
        builder.Property(address => address.Street).IsRequired().HasMaxLength(200);
        builder.Property(address => address.City).IsRequired().HasMaxLength(100);
        builder.Property(address => address.PostalCode).IsRequired().HasMaxLength(20);
        builder.Property(address => address.Country).IsRequired().HasMaxLength(100);

        builder.HasOne(address => address.User)
            .WithMany(user => user.Addresses)
            .HasForeignKey(address => address.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasIndex(item => new { item.UserId, item.ProductId }).IsUnique();

        builder.HasOne(item => item.User)
            .WithMany(user => user.CartItems)
            .HasForeignKey(item => item.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
