using Microsoft.EntityFrameworkCore;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.Infrastructure.Persistence.Mappings;

public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderDetail> builder)
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
