using Microsoft.EntityFrameworkCore;

namespace GestorTareas
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

    }

}
