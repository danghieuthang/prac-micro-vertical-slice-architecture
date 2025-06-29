using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<TodoApp_Product_API>("product-api");
builder.AddProject<TodoApp_Order_API>("order-api");

builder.Build().Run();
