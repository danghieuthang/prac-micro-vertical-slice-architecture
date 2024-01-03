using Microsoft.EntityFrameworkCore;


namespace TodoApp.Order.Infrastructure.Persistence.Mappings;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.Property(product => product.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(product => product.CreateAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired();
    }
}
