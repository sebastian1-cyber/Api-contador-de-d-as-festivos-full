using apiFestivos.core.servicios;
using apiFestivos.dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Taller_1_Herramientas.Controllers
{
    [Route("api/tipo")]
    [ApiController]
    public class TipoControlador : ControllerBase
    {
        private readonly ITipoServicio servicio;

        public TipoControlador(ITipoServicio servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Tipo>))]
        public async Task<ActionResult<IEnumerable<Tipo>>> ObtenerTodos()
        {
            return Ok(await servicio.ObtenerTodos());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tipo))]
        public async Task<ActionResult<Tipo>> ObtenerPorId(int id)
        {
            var tipo = await servicio.ObtenerPorId(id);
            if (tipo == null)
                return NotFound(new { mensaje = $"No se encontró el Tipo con ID= {id}" });
            return Ok(tipo);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Tipo))]
        public async Task<ActionResult<Tipo>> Agregar([FromBody] Tipo tipo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var nuevo = await servicio.Crear(tipo);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tipo))]
        public async Task<ActionResult<Tipo>> Actualizar(int id, [FromBody] Tipo tipo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existente = await servicio.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = $"No se encontró el Tipo con ID= {id}" });

            tipo.Id = id;
            await servicio.Actualizar(tipo);
            return Ok(tipo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {
            var existente = await servicio.ObtenerPorId(id);
            if (existente == null)
                return NotFound(new { mensaje = $"No se encontró el Tipo con ID= {id}" });

            await servicio.Eliminar(id);
            return Ok(true);
        }
    }
}
