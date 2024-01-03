using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Mappings;
public class EntitiesTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity<TId>
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
    }
}
