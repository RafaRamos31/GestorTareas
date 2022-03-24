using GestorTareas.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorTareas.Entities
{
    public class Tarea
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [MaxLength(100,ErrorMessage = "El campo {0} tiene un maximo de {1} caracteres.")]
        [MinLength(8, ErrorMessage = "El campo {0} tiene un minimo de {1} caracteres.")]
        [Parrafo]
        public String Descripcion { get; set; }

        public Usuario usuario { get; set; }
        public Categoria categoria { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        [NotMapped]
        public string newUsuario { get; set; }

        [NotMapped]
        public string newCategoria { get; set; }

    }
}
