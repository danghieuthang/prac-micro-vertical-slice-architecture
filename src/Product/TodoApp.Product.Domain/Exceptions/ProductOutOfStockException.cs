namespace TodoApp.Product.Domain.Exceptions;

public class ProductOutOfStockException : Exception
{
    public ProductOutOfStockException(string message) : base(message)
    {
    }
}