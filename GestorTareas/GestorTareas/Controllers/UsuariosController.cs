using GestorTareas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Controllers
{
    [ApiController]
    [Route("/api/v1/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public UsuariosController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            return await context.Usuarios.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Usuario usuario)
        {
            context.Add(usuario);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Usuario usuario, int id)
        {
            var existe = await context.Usuarios.AnyAsync(u => u.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            if (usuario.Id != id)
            {
                return BadRequest("El identificador del Usuario no coincide con la URL");
            }

            context.Update(usuario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Usuarios.AnyAsync(u => u.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Usuario { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
