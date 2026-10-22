namespace NorthWind.Sales.Frontend.WebApiGateways;

using System.Net.Http.Json;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Entities.Dtos.CreateOrder;

internal class CreateOrderGateway(HttpClient client) : ICreateOrderGateway
{
    public async Task<int> CreateOrderAsync(CreateOrderDto order)
    {
        var Response = await client.PostAsJsonAsync(
            Endpoints.CreateOrder, order);

        return await Response.Content.ReadFromJsonAsync<int>();
    }
}