using GestorTareas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GestorTareas.Controllers
{
    [ApiController]
    [Route("/api/v1/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public TareasController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tarea>>> Get([FromQuery] string usuario = null, [FromQuery] string categoria = null, [FromQuery] string terminadas = null)
        {
            var tareas =  await context.Tareas.Include(t => t.categoria).Include(t => t.usuario).ToListAsync();

            if(usuario != null)
            {
                Match regex = Regex.Match(usuario, "\\d+");
                if (regex.Success)
                {
                    tareas = tareas.Where(t => t.UsuarioId == Int32.Parse(usuario)).ToList();
                }
                else
                {
                    foreach (Tarea t in tareas)
                    {
                        regex = Regex.Match(t.usuario.Name, $".*{usuario}.*");
                        if (!regex.Success)
                        {
                            tareas.Remove(t);
                        }
                    }
                }
            }

            if (categoria != null)
            {
                Match regex = Regex.Match(categoria, "\\d+");
                if (regex.Success)
                {
                    tareas = tareas.Where(t => t.CategoriaId == Int32.Parse(categoria)).ToList();
                }
                else
                {
                    foreach (Tarea t in tareas)
                    {
                        regex = Regex.Match(t.categoria.Name, $".*{categoria.ToUpper()}.*");
                        if (!regex.Success)
                        {
                            tareas.Remove(t);
                        }
                    }
                }
            }

            if (terminadas != null)
            {
                if(terminadas == "false")
                {
                    tareas = tareas.Where(t => t.Estado == false).ToList();
                }
                if (terminadas == "true")
                {
                    tareas = tareas.Where(t => t.Estado == true).ToList();
                }
            }

            return tareas;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Tarea tarea)
        {
            var existeCategoria = await context.Categorias.AnyAsync(c => c.Id == tarea.CategoriaId);
            if (!existeCategoria)
            {
                return BadRequest($"No existe una categoria con el Id {tarea.CategoriaId}");
            }

            var existeUsuario = await context.Usuarios.AnyAsync(u => u.Id == tarea.UsuarioId);
            if (!existeUsuario)
            {
                return BadRequest($"No existe un usuario con el Id {tarea.UsuarioId}");
            }

            tarea.FechaInicio = DateTime.Now;
            tarea.Estado = false;

            context.Add(tarea);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Tarea tarea, int id)
        {
            var existe = await context.Tareas.AnyAsync(t => t.ID == id);
            if (!existe)
            {
                return NotFound();
            }

            if (tarea.ID != id)
            {
                return BadRequest("El identificador de la Tarea no coincide con la URL");
            }

            var existeCategoria = await context.Categorias.AnyAsync(c => c.Id == tarea.CategoriaId);
            if (!existeCategoria)
            {
                return BadRequest($"No existe una categoria con el Id {tarea.CategoriaId}");
            }

            var existeUsuario = await context.Usuarios.AnyAsync(u => u.Id == tarea.UsuarioId);
            if (!existeUsuario)
            {
                return BadRequest($"No existe un usuario con el Id {tarea.UsuarioId}");
            }

            context.Update(tarea);
            await context.SaveChangesAsync();
            return Ok();
        }

      
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Tareas.AnyAsync(t => t.ID == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Tarea { ID = id });
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
