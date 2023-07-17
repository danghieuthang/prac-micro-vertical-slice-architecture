using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Mappings;
public class EntitiesTypeConfiguration<TEntity, TID> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity<TID>
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
    }
}
