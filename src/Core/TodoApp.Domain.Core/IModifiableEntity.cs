namespace TodoApp.Domain.Core;

public interface IModifiableEntity
{
    DateTime? ModifiedAt { get; set; }
}