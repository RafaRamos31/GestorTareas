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
                //Se valida que el campo ingresado sea un valor numerico para compararlo con el ID
                Match regex = Regex.Match(usuario, "\\d+");
                if (regex.Success)
                {
                    tareas = tareas.Where(t => t.UsuarioId == Int32.Parse(usuario)).ToList();
                }
            }

            if (categoria != null)
            {
                Match regex = Regex.Match(categoria, "\\d+");
                if (regex.Success)
                {
                    tareas = tareas.Where(t => t.CategoriaId == Int32.Parse(categoria)).ToList();
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
            //Validar si se usara una categoria nueva o una existente
            if(tarea.newCategoria != null)
            {
                var exists = await context.Categorias.AnyAsync(c => c.Name == tarea.newCategoria.ToUpper());
                if (exists)
                {
                    return BadRequest($"Ya está registrada una categoria con el nombre {tarea.newCategoria.ToUpper()}");
                }

                context.Add(new Categoria { Name = tarea.newCategoria.ToUpper() });
                await context.SaveChangesAsync();

                Categoria categoria = context.Categorias.Where(c => c.Name == tarea.newCategoria.ToUpper()).ToList()[0];
                tarea.CategoriaId = categoria.Id;
                
            }
            else
            {
                var existeCategoria = await context.Categorias.AnyAsync(c => c.Id == tarea.CategoriaId);
                if (!existeCategoria)
                {
                    return BadRequest($"No existe una categoria con el Id {tarea.CategoriaId}");
                }
            }

            //Validar si se usara un usuario nuevo o uno existente
            if (tarea.newUsuario != null)
            {
                var existsUser = await context.Usuarios.AnyAsync(u => u.Name == tarea.newUsuario);
                if (existsUser)
                {
                    return BadRequest($"Ya está registrado un usuario con el nombre {tarea.newUsuario}");
                }

                context.Add(new Usuario { Name = tarea.newUsuario});
                await context.SaveChangesAsync();

                Usuario usuario = context.Usuarios.Where(u => u.Name == tarea.newUsuario).ToList()[0];
                tarea.UsuarioId = usuario.Id;

            }
            else
            {
                var existeUsuario = await context.Usuarios.AnyAsync(u => u.Id == tarea.UsuarioId);
                if (!existeUsuario)
                {
                    return BadRequest($"No existe un usuario con el Id {tarea.UsuarioId}");
                }
            }
            
            //Se inicializa con la fecha y hora actual de creacion y con estado de no finalizada
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

        //Llamada para marcar como finalizada una tarea

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Finish(int id)
        {
            var existe = await context.Tareas.AnyAsync(t => t.ID == id);
            if (!existe)
            {
                return NotFound();
            }

            Tarea tarea =  context.Tareas.Where(t => t.ID == id).ToList()[0];

            //El estado pasa a true y se pone la fecha y hora de finalizacion
            tarea.Estado = true;
            tarea.FechaFin = DateTime.Now;

            context.Update(tarea);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
