namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;

public interface IProductGateway
{
    Task<PagedResult<ProductDto>> SearchProductsAsync(string? query, string searchBy, int pageNumber, int pageSize);
}
