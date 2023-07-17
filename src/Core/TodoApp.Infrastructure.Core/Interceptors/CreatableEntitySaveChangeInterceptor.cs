using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Interceptors;
public class CreatableEntitySaveChangeInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData == null) throw new ArgumentNullException(nameof(eventData));

        var creatableEntries = eventData.Context?.ChangeTracker.Entries<ICreatableEntity>() ?? Enumerable.Empty<EntityEntry<ICreatableEntity>>();

        foreach (var entry in creatableEntries)
        {
            if (entry.State is not Microsoft.EntityFrameworkCore.EntityState.Added) continue;
            entry.Entity.CreateAt = DateTime.UtcNow;
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);

    }
}
