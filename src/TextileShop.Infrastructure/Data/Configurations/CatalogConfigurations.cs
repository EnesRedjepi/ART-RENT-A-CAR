using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextileShop.Domain.Entities;

namespace TextileShop.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(category => category.Name).IsRequired().HasMaxLength(100);
        builder.Property(category => category.Slug).IsRequired().HasMaxLength(120);
        builder.Property(category => category.Description).HasMaxLength(500);
        builder.HasIndex(category => category.Slug).IsUnique();
        builder.HasIndex(category => category.Name).IsUnique();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(product => product.Name).IsRequired().HasMaxLength(150);
        builder.Property(product => product.Description).HasMaxLength(4000);
        builder.Property(product => product.Sku).IsRequired().HasMaxLength(50);
        builder.Property(product => product.Brand).HasMaxLength(100);
        builder.Property(product => product.Material).HasMaxLength(100);
        builder.Property(product => product.Size).HasMaxLength(100);
        builder.Property(product => product.Color).HasMaxLength(100);
        builder.Property(product => product.StockQuantity).IsConcurrencyToken();

        builder.HasIndex(product => product.Sku).IsUnique();
        builder.HasIndex(product => new { product.IsActive, product.CategoryId });

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(product => product.Images)
            .WithOne(image => image.Product)
            .HasForeignKey(image => image.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
{
    public void Configure(EntityTypeBuilder<ProductTranslation> builder)
    {
        builder.Property(translation => translation.Culture).IsRequired().HasMaxLength(10);
        builder.Property(translation => translation.Name).IsRequired().HasMaxLength(150);
        builder.Property(translation => translation.Description).HasMaxLength(4000);
        builder.Property(translation => translation.Brand).HasMaxLength(100);
        builder.Property(translation => translation.Material).HasMaxLength(100);
        builder.Property(translation => translation.Color).HasMaxLength(100);
        builder.Property(translation => translation.Size).HasMaxLength(100);
        builder.HasIndex(translation => new { translation.ProductId, translation.Culture }).IsUnique();

        builder.HasOne(translation => translation.Product)
            .WithMany(product => product.Translations)
            .HasForeignKey(translation => translation.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.Property(translation => translation.Culture).IsRequired().HasMaxLength(10);
        builder.Property(translation => translation.Name).IsRequired().HasMaxLength(100);
        builder.Property(translation => translation.Description).HasMaxLength(500);
        builder.HasIndex(translation => new { translation.CategoryId, translation.Culture }).IsUnique();

        builder.HasOne(translation => translation.Category)
            .WithMany(category => category.Translations)
            .HasForeignKey(translation => translation.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(image => image.FileName).IsRequired().HasMaxLength(200);
    }
}
