using Microsoft.EntityFrameworkCore;

namespace TodoApp.Product.API.Infrastructure.Mappings;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<TodoApp.Product.Domain.Entities.Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TodoApp.Product.Domain.Entities.Product> builder)
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
