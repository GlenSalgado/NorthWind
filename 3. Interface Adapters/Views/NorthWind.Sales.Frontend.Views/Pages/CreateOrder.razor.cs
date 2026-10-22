namespace NorthWind.Sales.Frontend.Views.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public partial class CreateOrder
{
    [Inject]
    CreateOrderViewModel ViewModel { get; set; }

    ErrorBoundary ErrorBoundaryRef;

    void Recover()
    {
        ErrorBoundaryRef?.Recover();
    }
}