namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class InvoiceDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;

    public InvoiceDetailDto() { }

    public InvoiceDetailDto(int productId, string productName, decimal unitPrice, short quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
