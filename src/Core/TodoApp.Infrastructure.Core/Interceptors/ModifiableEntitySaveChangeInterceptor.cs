using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Interceptors;
public class ModifiableEntitySaveChangeInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData == null) throw new ArgumentNullException(nameof(eventData));

        var modifiableEntries = eventData.Context?.ChangeTracker.Entries<IModifiableEntity>() ?? Enumerable.Empty<EntityEntry<IModifiableEntity>>();

        foreach (var entry in modifiableEntries)
        {
            if (entry.State is not (EntityState.Modified or EntityState.Detached)) continue;
            entry.Entity.ModifiedAt = DateTime.UtcNow;
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
    }
}
