namespace Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.ValueObjects;

public static class CustomerQueryController
{
    public static WebApplication UseCustomerQueryController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, SearchCustomers);
        return app;
    }

    public static async Task<IResult> SearchCustomers(
        [FromQuery] string? query,
        [FromQuery] string? searchBy,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        IQueriesRepository repository)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        PagedResult<CustomerDto> result = await repository.SearchCustomers(query, searchBy ?? "all", pageNumber, pageSize);
        return Results.Ok(result);
    }
}
