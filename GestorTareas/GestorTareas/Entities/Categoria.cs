using GestorTareas.Validations;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Entities
{
    public class Categoria
    {
        public int Id { get; set;}

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} tiene un maximo de {1} caracteres.")]
        [MinLength(3, ErrorMessage = "El campo {0} tiene un minimo de {1} caracteres.")]
        [Parrafo]
        public string Name { get; set;}

    }
}
