using GestorTareas.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Controllers
{
    [ApiController]
    [Route("api/v1/tareas")]
    public class TareaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Tarea>> Get()
        {
            return new List<Tarea>()
            {
                new Tarea { ID = 1, Descripcion="Reparacion"}
            };
        }
    }
}
