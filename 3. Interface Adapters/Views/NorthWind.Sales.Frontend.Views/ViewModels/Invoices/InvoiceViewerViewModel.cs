namespace NorthWind.Sales.Frontend.Views.ViewModels.Invoices;

using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public class InvoiceViewerViewModel(IOrderQueryGateway gateway)
{
    public PagedResult<OrderSummaryDto>? OrdersPage { get; set; }
    public InvoiceDto? CurrentInvoice { get; set; }
    public string SearchText { get; set; } = string.Empty;
    public string SearchBy { get; set; } = "all";
    public int CurrentPage { get; set; } = 1;
    public const int PageSize = 5;
    public bool IsLoading { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public async Task SearchOrders(int page = 1)
    {
        CurrentPage = page;
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            OrdersPage = await gateway.SearchOrdersAsync(SearchText, SearchBy, CurrentPage, PageSize);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al buscar órdenes de venta: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task SelectInvoice(int orderId)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            CurrentInvoice = await gateway.GetInvoiceAsync(orderId);
            if (CurrentInvoice == null)
            {
                ErrorMessage = $"No se encontró la factura con el número #{orderId}.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al reconstruir la factura #{orderId}: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ClearSelectedInvoice()
    {
        CurrentInvoice = null;
    }
}
