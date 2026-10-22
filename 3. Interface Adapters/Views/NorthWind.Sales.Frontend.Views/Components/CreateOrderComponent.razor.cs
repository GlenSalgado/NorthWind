namespace NorthWind.Sales.Frontend.Views.Components;

using Microsoft.AspNetCore.Components;
using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public partial class CreateOrderComponent
{
    [Parameter]
    public CreateOrderViewModel Order { get; set; }
}