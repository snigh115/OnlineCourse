using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StudentRegisteration.Validators
{
    public class NrcValidation
    {
       public static ValidationResult ValidateNrcNumber(string nrcNumber, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(nrcNumber))
            {
                return new ValidationResult("NRC Number is required");
            }

            string pattern = @"^([0-9]{1,2})\/([A-Z][a-z]|[A-Z][a-z][a-z])([A-Z][a-z]|[A-Z][a-z][a-z])([A-Z][a-z]|[A-Z][a-z][a-z])\([N,P,E]\)[0-9]{6}$";
            // string pattern = @"\d{1,2}/{A-Za-z}(3)[A-Za-z]{0,2}[A-Za-z]{0,2}[NPE]\d{6}$";

            if (!Regex.IsMatch(nrcNumber, pattern))
            {
                return new ValidationResult("Invalid NRC number format.");
            }

            return ValidationResult.Success;
        }
        
    }
}
