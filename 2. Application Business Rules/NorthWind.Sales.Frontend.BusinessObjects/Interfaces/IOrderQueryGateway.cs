namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;

public interface IOrderQueryGateway
{
    Task<PagedResult<OrderSummaryDto>> SearchOrdersAsync(string? query, string searchBy, int pageNumber, int pageSize);
    Task<InvoiceDto?> GetInvoiceAsync(int orderId);
}
