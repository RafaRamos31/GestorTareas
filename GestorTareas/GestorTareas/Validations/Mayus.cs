using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Validations
{
    public class Mayus : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var palabras = value.ToString().Split(' ');
            foreach (var palabra in palabras)
            {
                var primraletra = palabra.ToString()[0].ToString();
                if (primraletra != primraletra.ToUpper())
                {
                    return new ValidationResult("La primera letra del Nombre y Apellido debe ser mayuscula");
                }
            }
            return ValidationResult.Success;
        }
    }
}
