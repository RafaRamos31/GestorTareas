using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Validations
{
    public class Parrafo : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var inicio = value.ToString().Split(' ');

            var primerapalabra = inicio[0].ToString();

            var primeraletra = primerapalabra[0].ToString();
            if (primeraletra != primeraletra.ToUpper())
            {
                return new ValidationResult("La primera letra del parrafo debe ser mayuscula");
            }
            return ValidationResult.Success;
        }
    }
}
