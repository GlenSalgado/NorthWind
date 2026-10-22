namespace NorthWind.Sales.Backend.Repositories.Repositories;

using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.ValueObjects;
using NorthWind.Sales.Backend.Repositories.Interfaces;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Invoices;
using NorthWind.Sales.Entities.Dtos.Pagination;
using NorthWind.Sales.Entities.Dtos.Products;

internal class QueriesRepository(INorthWindSalesQueriesDataContext context)
    : IQueriesRepository
{
    public async Task<decimal?> GetCustomerCurrentBalance(string customerId)
    {
        var queryable = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => new { c.CurrentBalance });

        var result = await context.FirstOrDefaultAsync(queryable);
        return result?.CurrentBalance;
    }

    public async Task<IEnumerable<ProductUnitsInStock>> GetProductsUnitsInStock(
        IEnumerable<int> productIds)
    {
        var queryable = context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock));

        return await context.ToListAsync(queryable);
    }

    public async Task<PagedResult<CustomerDto>> SearchCustomers(string? query, string searchBy, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        IQueryable<Entities.Customer> queryable = context.Customers;

        if (!string.IsNullOrWhiteSpace(query))
        {
            var trimmed = query.Trim().ToLower();
            queryable = (searchBy ?? "all").ToLowerInvariant() switch
            {
                "id" => queryable.Where(c => c.Id.ToLower().Contains(trimmed)),
                "name" => queryable.Where(c => c.Name.ToLower().Contains(trimmed)),
                "cedula" => queryable.Where(c => c.Cedula != null && c.Cedula.Contains(trimmed)),
                _ => queryable.Where(c => c.Id.ToLower().Contains(trimmed) ||
                    c.Name.ToLower().Contains(trimmed) ||
                    (c.Cedula != null && c.Cedula.Contains(trimmed)))
            };
        }

        int totalItems = await context.CountAsync(queryable);

        var pagedQuery = queryable
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto(c.Id, c.Name, c.CurrentBalance, c.Cedula, c.Email));

        var items = (await context.ToListAsync(pagedQuery)).ToList();

        return new PagedResult<CustomerDto>(items, totalItems, pageNumber, pageSize);
    }

    public async Task<PagedResult<ProductDto>> SearchProducts(string? query, string searchBy, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        // Requirement 8: ONLY products with UnitsInStock > 0
        IQueryable<Entities.Product> queryable = context.Products.Where(p => p.UnitsInStock > 0);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var trimmed = query.Trim().ToLower();
            queryable = (searchBy ?? "all").ToLowerInvariant() switch
            {
                "id" => int.TryParse(trimmed, out var id) ? queryable.Where(p => p.Id == id) : queryable.Where(p => false),
                "stock" => short.TryParse(trimmed, out var stock) ? queryable.Where(p => p.UnitsInStock == stock) : queryable.Where(p => false),
                "price" => decimal.TryParse(trimmed, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var price)
                    ? queryable.Where(p => p.UnitPrice == price)
                    : decimal.TryParse(trimmed, out price) ? queryable.Where(p => p.UnitPrice == price) : queryable.Where(p => false),
                "name" => queryable.Where(p => p.Name.ToLower().Contains(trimmed)),
                _ => queryable.Where(p => p.Name.ToLower().Contains(trimmed) || p.Id.ToString().Contains(trimmed))
            };
        }

        int totalItems = await context.CountAsync(queryable);

        var pagedQuery = queryable
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.UnitsInStock, p.UnitPrice));

        var items = (await context.ToListAsync(pagedQuery)).ToList();

        return new PagedResult<ProductDto>(items, totalItems, pageNumber, pageSize);
    }

    public async Task<PagedResult<OrderSummaryDto>> SearchOrders(string? query, string searchBy, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        var baseQuery = from o in context.Orders
                        join c in context.Customers on o.CustomerId equals c.Id
                        select new
                        {
                            o.Id,
                            o.OrderDate,
                            o.CustomerId,
                            CustomerName = c.Name,
                            o.ShipCity,
                            o.ShipCountry
                        };

        if (!string.IsNullOrWhiteSpace(query))
        {
            var trimmed = query.Trim().ToLower();
            var hasOrderNumber = int.TryParse(trimmed, out var anyOrderNumber);
            var hasSearchDate = DateTime.TryParse(trimmed, out var anySearchDate);
            baseQuery = (searchBy ?? "all").ToLowerInvariant() switch
            {
                "number" => int.TryParse(trimmed, out var orderId) ? baseQuery.Where(o => o.Id == orderId) : baseQuery.Where(o => false),
                "customer" => baseQuery.Where(o => o.CustomerId.ToLower().Contains(trimmed) || o.CustomerName.ToLower().Contains(trimmed)),
                "date" => DateTime.TryParse(trimmed, out var date) ? baseQuery.Where(o => o.OrderDate.Date == date.Date) : baseQuery.Where(o => false),
                "city" => baseQuery.Where(o => o.ShipCity.ToLower().Contains(trimmed)),
                _ => baseQuery.Where(o =>
                    (hasOrderNumber && o.Id == anyOrderNumber) ||
                    o.CustomerId.ToLower().Contains(trimmed) ||
                    o.CustomerName.ToLower().Contains(trimmed) ||
                    o.ShipCity.ToLower().Contains(trimmed) ||
                    o.ShipCountry.ToLower().Contains(trimmed) ||
                    (hasSearchDate && o.OrderDate.Date == anySearchDate.Date))
            };
        }

        int totalItems = await context.CountAsync(baseQuery);

        var pagedItemsQuery = baseQuery
            .OrderByDescending(o => o.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var pagedOrders = (await context.ToListAsync(pagedItemsQuery)).ToList();

        var orderIds = pagedOrders.Select(o => o.Id).ToList();

        var detailsQuery = context.OrderDetails
            .Where(d => orderIds.Contains(d.OrderId))
            .Select(d => new { d.OrderId, LineTotal = d.UnitPrice * d.Quantity });

        var details = (await context.ToListAsync(detailsQuery)).ToList();

        var summaries = pagedOrders.Select(o =>
        {
            var orderDetails = details.Where(d => d.OrderId == o.Id).ToList();
            decimal subtotal = orderDetails.Sum(d => d.LineTotal);
            decimal vat = Math.Round(subtotal * 0.15m, 2);
            decimal total = subtotal + vat;

            return new OrderSummaryDto
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                CustomerId = o.CustomerId,
                CustomerName = o.CustomerName,
                ShipCity = o.ShipCity,
                ShipCountry = o.ShipCountry,
                TotalItems = orderDetails.Count,
                TotalAmount = total
            };
        }).ToList();

        return new PagedResult<OrderSummaryDto>(summaries, totalItems, pageNumber, pageSize);
    }

    public async Task<InvoiceDto?> GetInvoiceById(int orderId)
    {
        var orderQuery = context.Orders.Where(o => o.Id == orderId);
        var order = await context.FirstOrDefaultAsync(orderQuery);
        if (order == null) return null;

        var customerQuery = context.Customers.Where(c => c.Id == order.CustomerId);
        var customer = await context.FirstOrDefaultAsync(customerQuery);

        var detailsQuery = from d in context.OrderDetails
                           join p in context.Products on d.ProductId equals p.Id
                           where d.OrderId == orderId
                           select new InvoiceDetailDto(d.ProductId, p.Name, d.UnitPrice, d.Quantity);

        var details = (await context.ToListAsync(detailsQuery)).ToList();

        return new InvoiceDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = customer?.Name ?? order.CustomerId,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipCountry = order.ShipCountry,
            ShipPostalCode = order.ShipPostalCode,
            Details = details,
            VatRate = 0.15m
        };
    }
}
