namespace NorthWind.Sales.Frontend.WebApiGateways;

using System.Net.Http.Json;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Entities.ValueObjects;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

internal class ProductGateway(HttpClient client) : IProductGateway
{
    public async Task<PagedResult<ProductDto>> SearchProductsAsync(string? query, string searchBy, int pageNumber, int pageSize)
    {
        var url = $"{Endpoints.SearchProducts}?query={Uri.EscapeDataString(query ?? "")}&searchBy={Uri.EscapeDataString(searchBy)}&pageNumber={pageNumber}&pageSize={pageSize}";
        var result = await client.GetFromJsonAsync<PagedResult<ProductDto>>(url);
        return result ?? new PagedResult<ProductDto>();
    }
}
