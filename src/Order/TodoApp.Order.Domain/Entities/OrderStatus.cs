namespace TodoApp.Order.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}
