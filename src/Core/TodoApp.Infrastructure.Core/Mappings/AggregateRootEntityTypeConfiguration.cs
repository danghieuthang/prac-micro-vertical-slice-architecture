using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Mappings;
public class AggregateRootEntityTypeConfiguration<TId> : IEntityTypeConfiguration<AggregateRoot<TId>>
{
    public void Configure(EntityTypeBuilder<AggregateRoot<TId>> builder)
    {
        builder.Ignore(aggregate => aggregate.DomainEvents);
    }
}
