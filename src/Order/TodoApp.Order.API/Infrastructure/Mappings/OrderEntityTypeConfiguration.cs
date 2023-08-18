using Microsoft.EntityFrameworkCore;


namespace TodoApp.Order.API.Infrastructure.Mappings;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<order.Order>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<order.Order> builder)
    {
        builder.Property(product => product.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(product => product.CreateAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired();
    }
}
