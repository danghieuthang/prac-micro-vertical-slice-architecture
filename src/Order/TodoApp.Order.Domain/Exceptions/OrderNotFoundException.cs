namespace TodoApp.Order.Domain.Exceptions;
public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(string message) : base(message)
    {

    }
}
