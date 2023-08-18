using Microsoft.EntityFrameworkCore;

namespace TodoApp.Order.API.Infrastructure.Mappings;

public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<order.OrderDetail>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<order.OrderDetail> builder)
    {
        builder.Property(product => product.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Ignore(product => product.LineTotal);

        builder.Property(product => product.CreateAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired();
    }
}
