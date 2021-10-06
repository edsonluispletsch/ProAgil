using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;
        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
        /*        [HttpPost("upload")]
                public async Task<IActionResult> upload()
                {
                    try
                    {
                        //var file = Request.Form.Files[0];
                        var folderName = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        //if (file.Length > 0)
                        {
                            //var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                            var filename = "Teste.jpg";
                            var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                await Request.Form.Files[0].CopyToAsync(stream);
                                //      await file.CopyToAsync(stream);
                            }
                            return Ok();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
                    }

                    //return BadRequest("Erro ao tentar realizar upload");
                }*/



        /*        [HttpPost("upload")]
                public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
                {
                    long size = files.Sum(f => f.Length);

                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var folderName = Path.Combine("Resources", "Images");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            var filePath = Path.GetTempFileName();
                            var pathToSaveComp = Path.Combine(filePath, folderName);

                            using (var stream = System.IO.File.Create(pathToSaveComp))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }

                    // Process uploaded files
                    // Don't rely on or trust the FileName property without validation.

                    return Ok(new { count = files.Count, size });
                }*/


        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null) return BadRequest("NULL FILE");
                if (file.Length == 0) return BadRequest("Empty File");
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar upload da imagem {ex.Message}");
            }
        }

        [HttpGet("{EventoID}")]
        public async Task<IActionResult> Get(int EventoID)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncByID(EventoID, true);
                var results = _mapper.Map<EventoDto>(evento);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }
        }
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
        [HttpPost()]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.ID}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }

            return BadRequest();
        }
        [HttpPut("{EventoID}")]
        public async Task<IActionResult> Put(int EventoID, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncByID(EventoID, false);
                if (evento == null) return NotFound();

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();

                model.Lotes.ForEach(item => idLotes.Add(item.ID));
                model.RedesSociais.ForEach(item => idRedesSociais.Add(item.ID));

                var lotes = evento.Lotes.Where(
                    lote => !idLotes.Contains(lote.ID)
                ).ToArray();

                var redesSociais = evento.RedesSociais.Where(
                    rede => !idRedesSociais.Contains(rede.ID)
                ).ToArray();

                if (lotes.Length > 0) _repo.DeleteRange(lotes);
                if (redesSociais.Length > 0) _repo.DeleteRange(redesSociais);

                _mapper.Map(model, evento);

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    //    return Ok();
                    return Created($"/api/evento/{model.ID}", _mapper.Map<EventoDto>(evento));
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