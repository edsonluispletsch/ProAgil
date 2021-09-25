using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllEventoAsync(true);
                return Ok(results); 
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }          
        [HttpGet("{EventoID}")]
        public async Task<IActionResult> Get(int EventoID)
        {
            try
            {
                var results = await _repo.GetEventoAsyncByID(EventoID, true);
                return Ok(results); 
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }          
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncByTema(tema, true);
                return Ok(results); 
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }          
        [HttpPost()]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repo.Add(model);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.ID}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }      
        [HttpPut("{EventoID}")]
        public async Task<IActionResult> Put(int EventoID, Evento model)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncByID(EventoID, false);
                if (evento == null) return NotFound();

                //evento.Tema = model.Tema;

                _repo.Update(model);

                if (await _repo.SaveChangesAsync())
                {
                //    return Ok();
                    return Created($"/api/evento/{model.ID}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }         
        [HttpDelete("{EventoID}")]
        public async Task<IActionResult> Delete(int EventoID)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncByID(EventoID, false);
                if (evento == null) return NotFound();

                _repo.Delete(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }         

    }
}