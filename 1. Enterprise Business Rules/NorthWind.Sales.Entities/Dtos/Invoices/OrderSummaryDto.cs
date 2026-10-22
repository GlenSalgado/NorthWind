namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
}
