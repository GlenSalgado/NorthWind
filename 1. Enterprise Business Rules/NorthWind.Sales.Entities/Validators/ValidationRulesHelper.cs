namespace NorthWind.Sales.Entities.Validators;

using System.Text.RegularExpressions;

public static class ValidationRulesHelper
{
    static readonly Regex LettersRegex = new(@"^[\p{L}\s]+$", RegexOptions.Compiled);
    static readonly Regex NumbersRegex = new(@"^[0-9]+$", RegexOptions.Compiled);
    static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    static readonly Regex AddressRegex = new(@"^[\p{L}\p{N}\s#.,/º°-]+$", RegexOptions.Compiled);

    public static bool IsNotBlank(string? value) =>
        !string.IsNullOrWhiteSpace(value);

    public static bool IsOnlyLetters(string? value) =>
        !string.IsNullOrWhiteSpace(value) && LettersRegex.IsMatch(value.Trim());

    public static bool IsOnlyNumbers(string? value) =>
        !string.IsNullOrWhiteSpace(value) && NumbersRegex.IsMatch(value.Trim());

    public static bool IsValidEmail(string? value) =>
        !string.IsNullOrWhiteSpace(value) && EmailRegex.IsMatch(value.Trim());

    public static bool IsValidAddress(string? value) =>
        !string.IsNullOrWhiteSpace(value) && AddressRegex.IsMatch(value.Trim());

    /// <summary>
    /// Validates an Ecuadorian 10-digit national identification card (Cédula de Identidad)
    /// using the standard Module 10 algorithm.
    /// </summary>
    public static bool IsValidEcuadorianCedula(string? cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula))
            return false;

        cedula = cedula.Trim();

        if (cedula.Length != 10 || !NumbersRegex.IsMatch(cedula))
            return false;

        int province = int.Parse(cedula[..2]);
        if ((province < 1 || province > 24) && province != 30)
            return false;

        int thirdDigit = int.Parse(cedula[2].ToString());
        if (thirdDigit >= 6)
            return false;

        int[] coefficients = [2, 1, 2, 1, 2, 1, 2, 1, 2];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            int digit = int.Parse(cedula[i].ToString());
            int product = digit * coefficients[i];
            if (product >= 10)
                product -= 9;
            sum += product;
        }

        int remainder = sum % 10;
        int expectedVerifier = remainder == 0 ? 0 : 10 - remainder;
        int actualVerifier = int.Parse(cedula[9].ToString());

        return expectedVerifier == actualVerifier;
    }
}
