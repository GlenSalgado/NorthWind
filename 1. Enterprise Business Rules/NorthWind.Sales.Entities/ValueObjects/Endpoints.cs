namespace NorthWind.Sales.Entities.ValueObjects;

public class Endpoints
{
    public const string CreateOrder = $"/{nameof(CreateOrder)}";
    public const string SearchCustomers = "/api/customers/search";
    public const string SearchProducts = "/api/products/search";
    public const string SearchOrders = "/api/orders/search";
    public const string GetInvoice = "/api/orders";
}