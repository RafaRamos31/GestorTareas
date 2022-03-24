using GestorTareas.Validations;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} tiene un maximo de {1} caracteres.")]
        [MinLength(2, ErrorMessage = "El campo {0} tiene un minimo de {1} caracteres.")]
        [Mayus]
        public string Name { get; set; }    
    }
}
