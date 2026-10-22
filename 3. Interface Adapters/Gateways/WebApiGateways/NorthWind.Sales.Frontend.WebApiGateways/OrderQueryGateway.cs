namespace NorthWind.Sales.Frontend.WebApiGateways;

using System.Net.Http.Json;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

internal class OrderQueryGateway(HttpClient client) : IOrderQueryGateway
{
    public async Task<PagedResult<OrderSummaryDto>> SearchOrdersAsync(string? query, string searchBy, int pageNumber, int pageSize)
    {
        var url = $"{Endpoints.SearchOrders}?query={Uri.EscapeDataString(query ?? "")}&searchBy={Uri.EscapeDataString(searchBy)}&pageNumber={pageNumber}&pageSize={pageSize}";
        var result = await client.GetFromJsonAsync<PagedResult<OrderSummaryDto>>(url);
        return result ?? new PagedResult<OrderSummaryDto>();
    }

    public async Task<InvoiceDto?> GetInvoiceAsync(int orderId)
    {
        var url = $"{Endpoints.GetInvoice}/{orderId}";
        return await client.GetFromJsonAsync<InvoiceDto>(url);
    }
}
