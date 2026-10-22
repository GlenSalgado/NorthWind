namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;

public interface IQueriesRepository
{
    Task<decimal?> GetCustomerCurrentBalance(string customerId);
    Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(
        IEnumerable<int> productIds);
    Task<PagedResult<CustomerDto>> SearchCustomers(string? query, string searchBy, int pageNumber, int pageSize);
    Task<PagedResult<ProductDto>> SearchProducts(string? query, string searchBy, int pageNumber, int pageSize);
    Task<PagedResult<OrderSummaryDto>> SearchOrders(string? query, string searchBy, int pageNumber, int pageSize);
    Task<InvoiceDto?> GetInvoiceById(int orderId);
}
