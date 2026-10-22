namespace NorthWind.Sales.Entities.Dtos.Customers;

public class CustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public CustomerDto() { }

    public CustomerDto(string id, string name, decimal currentBalance, string? cedula = null, string? email = null)
    {
        Id = id;
        Name = name;
        CurrentBalance = currentBalance;
        Cedula = string.IsNullOrWhiteSpace(cedula) ? GetDefaultCedula(id) : cedula;
        Email = string.IsNullOrWhiteSpace(email) ? GetDefaultEmail(id) : email;
    }

    /// <summary>
    /// Returns 100% mathematically valid Ecuadorian identity card numbers (Cédulas)
    /// verified with the Civil Registry Module 10 algorithm.
    /// </summary>
    public static string GetDefaultCedula(string? id) => id?.Trim().ToUpperInvariant() switch
    {
        "ALFKI" => "1851076057", // Tungurahua - Ambato (Valid Módulo 10)
        "ANATR" => "1710034065", // Pichincha - Quito (Valid Módulo 10)
        "ANTON" => "0102030400", // Azuay - Cuenca (Valid Módulo 10)
        "AROUT" => "1801234566", // Tungurahua - Ambato (Valid Módulo 10)
        "BERGS" => "1802345676", // Tungurahua - Ambato (Valid Módulo 10)
        "BLAUS" => "1803456787", // Tungurahua - Ambato (Valid Módulo 10)
        "BLONP" => "1804567897", // Tungurahua - Ambato (Valid Módulo 10)
        "BOLID" => "1805678909", // Tungurahua - Ambato (Valid Módulo 10)
        "BONAP" => "1806789010", // Tungurahua - Ambato (Valid Módulo 10)
        "BOTTM" => "1807890122", // Tungurahua - Ambato (Valid Módulo 10)
        _ => "1851076057"
    };

    public static string GetDefaultEmail(string? id) =>
        string.IsNullOrWhiteSpace(id) ? "cliente@northwind.com.ec" : $"{id.Trim().ToLowerInvariant()}@northwind.com.ec";
}
