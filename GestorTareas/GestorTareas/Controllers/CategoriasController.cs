using GestorTareas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.Controllers
{
    [ApiController]
    [Route("/api/v1/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public CategoriasController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            return await context.Categorias.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            var existeCategoria = await context.Categorias.AnyAsync(c => c.Name == categoria.Name.ToUpper());
            if (existeCategoria)
            {
                return BadRequest($"Ya está registrada una categoria con el nombre {categoria.Name.ToUpper()}");
            }

            categoria.Name = categoria.Name.ToUpper();
            context.Add(categoria);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Categoria categoria, int id)
        {
            var existe = await context.Categorias.AnyAsync(c => c.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            if (categoria.Id != id)
            {
                return BadRequest("El identificador de la Categoria no coincide con la URL");
            }

            var existeCategoria = await context.Categorias.AnyAsync(c => c.Name == categoria.Name.ToUpper());
            if (existeCategoria)
            {
                return BadRequest($"Ya está registrada una categoria con el nombre {categoria.Name.ToUpper()}");
            }

            categoria.Name = categoria.Name.ToUpper();
            context.Update(categoria);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Categorias.AnyAsync(c => c.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Categoria { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
