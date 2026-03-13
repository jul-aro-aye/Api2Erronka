using ErronkaApi.DTOak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Kontrollerrak
{
    [ApiController]
    [Route("api/produktuak")]
    public class ProduktuakKontrollera : ControllerBase
    {
        private readonly ProduktuaRepository _repo;

        public ProduktuakKontrollera(ProduktuaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult LortuProduktuak()
        {
            var (success, error, data) = _repo.LortuProduktuak();

            if (!success)
                return StatusCode(500, new ErantzunaDTO<string> { Code = 500, Message = error });

            return Ok(new ErantzunaDTO<ProduktuaDTO>
            {
                Code = 200,
                Message = "Produktuak lortu dira",
                Datuak = data
            });
        }

        [HttpGet("{id}")]
        public IActionResult LortuProduktua(int id)
        {
            var (success, error, data) = _repo.LortuProduktua(id);

            if (!success)
                return NotFound(new ErantzunaDTO<string> { Code = 404, Message = error });

            return Ok(new ErantzunaDTO<ProduktuaDTO>
            {
                Code = 200,
                Message = "Produktua lortu da",
                Datuak = new List<ProduktuaDTO> { data! }
            });
        }

        [HttpPost]
        public IActionResult GehituProduktua([FromBody] ProduktuaDTO dto)
        {
            var (success, error) = _repo.GehituProduktua(dto);

            if (!success)
                return BadRequest(new ErantzunaDTO<string> { Code = 400, Message = error });

            return Ok(new ErantzunaDTO<string>
            {
                Code = 200,
                Message = "Produktua gehituta"
            });
        }

        [HttpPut("{id}")]
        public IActionResult EguneratuProduktua(int id, [FromBody] ProduktuaDTO dto)
        {
            var (success, error) = _repo.EguneratuProduktua(id, dto);

            if (!success)
                return NotFound(new ErantzunaDTO<string> { Code = 404, Message = error });

            return Ok(new ErantzunaDTO<string>
            {
                Code = 200,
                Message = "Produktua eguneratuta"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult EzabatuProduktua(int id)
        {
            var (success, error) = _repo.EzabatuProduktua(id);

            if (!success)
                return NotFound(new ErantzunaDTO<string> { Code = 404, Message = error });

            return Ok(new ErantzunaDTO<string>
            {
                Code = 200,
                Message = "Produktua ezabatuta"
            });
        }
    }
}
