namespace NorthWind.Sales.Frontend.WebApiGateways;

using System.Net.Http.Json;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

internal class CustomerGateway(HttpClient client) : ICustomerGateway
{
    public async Task<PagedResult<CustomerDto>> SearchCustomersAsync(string? query, string searchBy, int pageNumber, int pageSize)
    {
        var url = $"{Endpoints.SearchCustomers}?query={Uri.EscapeDataString(query ?? "")}&searchBy={Uri.EscapeDataString(searchBy)}&pageNumber={pageNumber}&pageSize={pageSize}";
        var result = await client.GetFromJsonAsync<PagedResult<CustomerDto>>(url);
        return result ?? new PagedResult<CustomerDto>();
    }
}
