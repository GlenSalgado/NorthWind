namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;

public interface ICustomerGateway
{
    Task<PagedResult<CustomerDto>> SearchCustomersAsync(string? query, string searchBy, int pageNumber, int pageSize);
}
