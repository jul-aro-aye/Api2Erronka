using ErronkaApi.DTOak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Kontrollerrak
{
    [ApiController]
    [Route("api/mahaiak")]
    public class MahaiakController : ControllerBase
    {
        private readonly MahaiaRepository _repo;

        public MahaiakController(MahaiaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult LortuMahaiak([FromQuery] DateTime? data = null, [FromQuery] string? txanda = null)
        {
            var (success, error, dataResult) = _repo.LortuMahaiak(data, txanda);

            if (!success)
                return StatusCode(500, new ErantzunaDTO<string> { Code = 500, Message = error });

            return Ok(new ErantzunaDTO<MahaiaDTO>
            {
                Code = 200,
                Message = "Mahaiak lortu dira",
                Datuak = dataResult
            });
        }

        [HttpGet("libre")]
        public IActionResult LortuMahaiLibre([FromQuery] DateTime? data = null, [FromQuery] string? txanda = null)
        {
            var (success, error, dataResult) = _repo.LortuMahaiLibre(data, txanda);

            if (!success)
                return StatusCode(500, new ErantzunaDTO<string> { Code = 500, Message = error });

            if (dataResult == null || !dataResult.Any())
                return NotFound(new ErantzunaDTO<string> { Code = 404, Message = "Ez dago mahai librerik" });

            return Ok(new ErantzunaDTO<MahaiaDTO>
            {
                Code = 200,
                Message = "Mahai libreak lortu dira",
                Datuak = dataResult
            });
        }

        [HttpGet("{id}")]
        public IActionResult LortuMahaiBat(int id, [FromQuery] DateTime? data = null, [FromQuery] string? txanda = null)
        {
            var (success, error, dataResult) = _repo.LortuMahaiBat(id, data, txanda);

            if (!success)
                return NotFound(new ErantzunaDTO<string> { Code = 404, Message = error });

            return Ok(new ErantzunaDTO<MahaiaDTO>
            {
                Code = 200,
                Message = "Mahaia lortu da",
                Datuak = new List<MahaiaDTO> { dataResult! }
            });
        }
    }
}
