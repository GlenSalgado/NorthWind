namespace Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;

public static class OrderQueryController
{
    public static WebApplication UseOrderQueryController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchOrders, SearchOrders);
        app.MapGet($"{Endpoints.GetInvoice}/{{id:int}}", GetInvoice);
        return app;
    }

    public static async Task<IResult> SearchOrders(
        [FromQuery] string? query,
        [FromQuery] string? searchBy,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        IQueriesRepository repository)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        PagedResult<OrderSummaryDto> result = await repository.SearchOrders(query, searchBy ?? "all", pageNumber, pageSize);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetInvoice(
        int id,
        IQueriesRepository repository)
    {
        InvoiceDto? invoice = await repository.GetInvoiceById(id);
        if (invoice == null)
            return Results.NotFound(new { Message = $"Factura con número #{id} no encontrada." });

        return Results.Ok(invoice);
    }
}
