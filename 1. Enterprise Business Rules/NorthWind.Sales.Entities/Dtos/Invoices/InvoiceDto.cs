namespace NorthWind.Sales.Entities.Dtos.Invoices;

public class InvoiceDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipPostalCode { get; set; } = string.Empty;
    public List<InvoiceDetailDto> Details { get; set; } = [];

    public decimal Subtotal => Details.Sum(d => d.Subtotal);
    public decimal VatRate { get; set; } = 0.15m;
    public decimal Vat => Math.Round(Subtotal * VatRate, 2);
    public decimal Total => Subtotal + Vat;
}
