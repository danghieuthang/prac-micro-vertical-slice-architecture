using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Interceptors;
public class ModifiableEntitySaveChangeInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData == null) throw new ArgumentNullException(nameof(eventData));

        var modifiableEntries = eventData.Context?.ChangeTracker.Entries<IModifiableEntity>() ?? Enumerable.Empty<EntityEntry<IModifiableEntity>>();

        foreach (var entry in modifiableEntries)
        {
            if (entry.State is not Microsoft.EntityFrameworkCore.EntityState.Modified) continue;
            entry.Entity.ModifiedAt = DateTime.UtcNow;
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
    }

}
