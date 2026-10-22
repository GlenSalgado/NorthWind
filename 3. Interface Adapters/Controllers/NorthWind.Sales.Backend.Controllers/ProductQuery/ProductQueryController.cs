namespace Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Entities.ValueObjects;

public static class ProductQueryController
{
    public static WebApplication UseProductQueryController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchProducts, SearchProducts);
        return app;
    }

    public static async Task<IResult> SearchProducts(
        [FromQuery] string? query,
        [FromQuery] string? searchBy,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        IQueriesRepository repository)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        PagedResult<ProductDto> result = await repository.SearchProducts(query, searchBy ?? "all", pageNumber, pageSize);
        return Results.Ok(result);
    }
}
