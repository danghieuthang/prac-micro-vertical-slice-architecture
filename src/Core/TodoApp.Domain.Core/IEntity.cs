namespace TodoApp.Domain.Core;

public interface IEntity<TId>
{
    public TId Id { get; }
}
