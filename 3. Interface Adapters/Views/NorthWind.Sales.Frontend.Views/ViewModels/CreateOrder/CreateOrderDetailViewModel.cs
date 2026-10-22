namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderDetailViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; } = 1;
    public short UnitsInStock { get; set; } = 1;

    public decimal Subtotal => Math.Round(UnitPrice * Quantity, 2);

    public CreateOrderDetailViewModel() { }

    public CreateOrderDetailViewModel(int productId, string productName, decimal unitPrice, short quantity, short unitsInStock)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        UnitsInStock = unitsInStock;
    }
}