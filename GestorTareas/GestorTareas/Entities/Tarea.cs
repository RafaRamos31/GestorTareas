namespace GestorTareas.Entities
{
    public class Tarea
    {
        public int ID { get; set; }

        public String Descripcion { get; set; }

        public Usuario usuario { get; set; }
        public Categoria categoria { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }
        
    }
}
