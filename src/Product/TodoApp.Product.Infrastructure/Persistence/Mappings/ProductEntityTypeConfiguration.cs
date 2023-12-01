using Microsoft.EntityFrameworkCore;

namespace TodoApp.Product.Infrastructure.Persistence.Mappings;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product.Domain.Entities.Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product.Domain.Entities.Product> builder)
    {
        builder.Property(product => product.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(product => product.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(product => product.CreateAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired();
    }
}
