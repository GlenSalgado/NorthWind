namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

using System.Net.Http;
using NorthWind.RazorComponents.Validators;
using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Entities.Dtos.Customers;
using NorthWind.Sales.Entities.Dtos.Products;
using NorthWind.Sales.Entities.Validators;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.Views.Resources;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;

public class CreateOrderViewModel(
    ICreateOrderGateway gateway,
    IModelValidatorHub<CreateOrderViewModel> validator)
{
    #region Customer and Order Header Properties
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerCedula { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = "Ambato";
    public string ShipCountry { get; set; } = "Ecuador";
    public string ShipPostalCode { get; set; } = "180101";
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public CustomerDto? SelectedCustomer { get; private set; }
    #endregion

    #region Order Details (Detail Lines)
    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    #endregion

    #region Calculations (Subtotal, IVA, Total)
    public decimal Subtotal => Math.Round(OrderDetails.Sum(d => d.Subtotal), 2);
    public decimal VatRate { get; set; } = 0.15m; // 15% IVA Ecuador
    public decimal Vat => Math.Round(Subtotal * VatRate, 2);
    public decimal Total => Math.Round(Subtotal + Vat, 2);
    #endregion

    #region UI Status and Messages
    public string InformationMessage { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int? CreatedOrderId { get; set; }
    public bool IsSubmitting { get; set; }
    public List<string> ValidationErrors { get; set; } = [];
    #endregion

    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;
    public ModelValidator<CreateOrderViewModel> ModelValidatorComponentReference { get; set; }

    #region Customer Management
    public void SelectCustomer(CustomerDto customer)
    {
        SelectedCustomer = customer;
        CustomerId = customer.Id;
        CustomerName = customer.Name;
        CustomerCedula = !string.IsNullOrWhiteSpace(customer.Cedula) ? customer.Cedula : CustomerDto.GetDefaultCedula(customer.Id);
        CustomerEmail = !string.IsNullOrWhiteSpace(customer.Email) ? customer.Email : CustomerDto.GetDefaultEmail(customer.Id);
        if (string.IsNullOrWhiteSpace(ShipAddress))
        {
            ShipAddress = "Av. Los Chasquis y Río Payamino";
        }
        if (string.IsNullOrWhiteSpace(ShipCity))
        {
            ShipCity = "Ambato";
        }
        if (string.IsNullOrWhiteSpace(ShipCountry))
        {
            ShipCountry = "Ecuador";
        }
        if (string.IsNullOrWhiteSpace(ShipPostalCode))
        {
            ShipPostalCode = "180101";
        }
        ErrorMessage = string.Empty;
    }

    public void ClearCustomer()
    {
        SelectedCustomer = null;
        CustomerId = string.Empty;
        CustomerName = string.Empty;
        CustomerCedula = string.Empty;
        CustomerEmail = string.Empty;
    }
    #endregion

    #region Product Management
    public bool AddProduct(ProductDto product, short quantity = 1)
    {
        ErrorMessage = string.Empty;

        // Requirement 3: A product should appear ONLY ONCE in the order (no duplicates!)
        if (OrderDetails.Any(d => d.ProductId == product.Id))
        {
            ErrorMessage = $"El producto '{product.Name}' (ID: {product.Id}) ya está incluido en la orden. No se permiten productos duplicados.";
            return false;
        }

        // Requirement 8: Only products with stock > 0
        if (product.UnitsInStock <= 0)
        {
            ErrorMessage = $"El producto '{product.Name}' no tiene stock disponible para la venta.";
            return false;
        }

        // Requirement 9: Do not allow selling more than available stock
        if (quantity > product.UnitsInStock)
        {
            quantity = product.UnitsInStock;
        }

        OrderDetails.Add(new CreateOrderDetailViewModel(
            product.Id,
            product.Name,
            product.UnitPrice,
            quantity,
            product.UnitsInStock
        ));

        return true;
    }

    // Requirement 11: Allow updating a product (changing for another) without losing other data
    public bool UpdateProduct(CreateOrderDetailViewModel item, ProductDto newProduct)
    {
        ErrorMessage = string.Empty;

        // Check if the new product already exists in another row
        if (item.ProductId != newProduct.Id && OrderDetails.Any(d => d.ProductId == newProduct.Id))
        {
            ErrorMessage = $"No se puede cambiar al producto '{newProduct.Name}' porque ya existe en el detalle.";
            return false;
        }

        // Requirement 8: Only products with stock > 0
        if (newProduct.UnitsInStock <= 0)
        {
            ErrorMessage = $"El producto '{newProduct.Name}' no tiene stock disponible.";
            return false;
        }

        item.ProductId = newProduct.Id;
        item.ProductName = newProduct.Name;
        item.UnitPrice = newProduct.UnitPrice;
        item.UnitsInStock = newProduct.UnitsInStock;

        // Requirement 9: Validate stock limit for existing quantity
        if (item.Quantity > newProduct.UnitsInStock)
        {
            item.Quantity = newProduct.UnitsInStock;
        }

        return true;
    }

    // Requirement 10: Allow deleting a product from the order before saving
    public void RemoveOrderDetailItem(CreateOrderDetailViewModel item)
    {
        OrderDetails.Remove(item);
    }

    public void AddNewOrderDetailItem()
    {
        // Fallback for manual row add if needed
        OrderDetails.Add(new CreateOrderDetailViewModel());
    }
    #endregion

    #region Custom Validation Logic (Requirement 6)
    public bool ValidateInputs()
    {
        ValidationErrors.Clear();

        if (string.IsNullOrWhiteSpace(CustomerId))
        {
            ValidationErrors.Add("Debe seleccionar un cliente para la orden de venta.");
        }

        // Ecuadorian Cedula validation if entered
        if (!string.IsNullOrWhiteSpace(CustomerCedula))
        {
            if (!ValidationRulesHelper.IsValidEcuadorianCedula(CustomerCedula))
            {
                ValidationErrors.Add("La cédula ingresada no es válida para la República del Ecuador (10 dígitos numéricos con algoritmo módulo 10).");
            }
        }

        // Email validation if entered
        if (!string.IsNullOrWhiteSpace(CustomerEmail))
        {
            if (!ValidationRulesHelper.IsValidEmail(CustomerEmail))
            {
                ValidationErrors.Add("El correo electrónico del cliente no tiene un formato válido (ejemplo: cliente@dominio.com).");
            }
        }

        if (string.IsNullOrWhiteSpace(ShipAddress))
        {
            ValidationErrors.Add("La dirección de entrega es obligatoria.");
        }

        // Only letters for City and Country
        if (string.IsNullOrWhiteSpace(ShipCity))
        {
            ValidationErrors.Add("La ciudad es obligatoria.");
        }
        else if (!ValidationRulesHelper.IsOnlyLetters(ShipCity))
        {
            ValidationErrors.Add("El campo 'Ciudad' solo debe contener letras.");
        }

        if (string.IsNullOrWhiteSpace(ShipCountry))
        {
            ValidationErrors.Add("El país es obligatorio.");
        }
        else if (!ValidationRulesHelper.IsOnlyLetters(ShipCountry))
        {
            ValidationErrors.Add("El campo 'País' solo debe contener letras.");
        }

        // Only numbers for Postal Code if present
        if (!string.IsNullOrWhiteSpace(ShipPostalCode) && !ValidationRulesHelper.IsOnlyNumbers(ShipPostalCode))
        {
            ValidationErrors.Add("El código postal debe contener únicamente números.");
        }

        // Details validation
        if (OrderDetails.Count == 0)
        {
            ValidationErrors.Add("Debe agregar al menos un producto al detalle de la orden.");
        }

        // Duplicate check
        var duplicateIds = OrderDetails.GroupBy(d => d.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicateIds.Count != 0)
        {
            ValidationErrors.Add("La orden contiene productos duplicados. Cada producto debe aparecer solo una vez.");
        }

        // Stock check per item
        foreach (var item in OrderDetails)
        {
            if (item.Quantity <= 0)
            {
                ValidationErrors.Add($"La cantidad para el producto '{item.ProductName}' (ID: {item.ProductId}) debe ser mayor a 0.");
            }
            if (item.UnitsInStock > 0 && item.Quantity > item.UnitsInStock)
            {
                ValidationErrors.Add($"No se puede vender más de lo disponible en stock para '{item.ProductName}'. Solicitado: {item.Quantity}, Stock: {item.UnitsInStock}.");
            }
        }

        return ValidationErrors.Count == 0;
    }
    #endregion

    #region Order Submission
    public async Task Send()
    {
        InformationMessage = string.Empty;
        ErrorMessage = string.Empty;
        ValidationErrors.Clear();

        if (!ValidateInputs())
        {
            ErrorMessage = "Existen errores de validación en el formulario. Por favor corríjalos antes de guardar.";
            return;
        }

        IsSubmitting = true;

        try
        {
            var orderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            CreatedOrderId = orderId;
            InformationMessage = $"¡Transacción Exitosa! La Orden de Venta / Factura #{orderId} ha sido procesada y almacenada satisfactoriamente con afectación de inventario.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.Data.Contains("Errors") && ex.Data["Errors"] is IEnumerable<ValidationError> errors)
            {
                // Mostrar errores del servidor en la lista visible del formulario
                foreach (var error in errors)
                    ValidationErrors.Add(error.Message);

                // También pasarlos al componente de validación de Blazor (para campos editables)
                ModelValidatorComponentReference?.AddErrors(errors);

                ErrorMessage = "La operación fue rechazada por el servidor. Por favor revise los errores indicados.";
            }
            else
            {
                ErrorMessage = $"Error en la transacción: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error inesperado al guardar la orden: {ex.Message}";
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    public void Reset()
    {
        ClearCustomer();
        OrderDetails.Clear();
        InformationMessage = string.Empty;
        ErrorMessage = string.Empty;
        CreatedOrderId = null;
        ValidationErrors.Clear();
        ShipAddress = string.Empty;
        ShipCity = "Ambato";
        ShipCountry = "Ecuador";
        ShipPostalCode = "180101";
        OrderDate = DateTime.Now;
    }
    #endregion

    public static explicit operator CreateOrderDto(CreateOrderViewModel model) =>
        new(
            model.CustomerId,
            model.ShipAddress,
            model.ShipCity,
            model.ShipCountry,
            model.ShipPostalCode,
            model.OrderDetails.Select(d => new CreateOrderDetailDto(
                d.ProductId,
                d.UnitPrice,
                d.Quantity
            ))
        );
}