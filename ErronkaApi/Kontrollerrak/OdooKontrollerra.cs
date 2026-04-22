using ErronkaApi.DTOak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Kontrollerrak
{
    [ApiController]
    [Route("api/odoo")]
    public class OdooKontrollerra : ControllerBase
    {
        private readonly OdooRepository _repo;

        public OdooKontrollerra(OdooRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("sinkronizazioa")]
        public IActionResult LortuSinkronizazioDatuak()
        {
            var (success, error, data) = _repo.LortuSinkronizazioDatuak();

            if (!success || data == null)
            {
                return StatusCode(500, new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = error ?? "Errorea sinkronizazio datuak eskuratzean",
                    Datuak = null
                });
            }

            return Ok(new ErantzunaDTO<OdooSyncDTO>
            {
                Code = 200,
                Message = "Odoorako sinkronizazio datuak eskuratu dira",
                Datuak = new List<OdooSyncDTO> { data }
            });
        }
    }
}
