namespace NorthWind.Sales.Entities.Dtos.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public short UnitsInStock { get; set; }
    public decimal UnitPrice { get; set; }

    public ProductDto() { }

    public ProductDto(int id, string name, short unitsInStock, decimal unitPrice)
    {
        Id = id;
        Name = name;
        UnitsInStock = unitsInStock;
        UnitPrice = unitPrice;
    }
}
