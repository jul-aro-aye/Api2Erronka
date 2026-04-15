using ErronkaApi.DTOak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Kontrollerrak
{
    [ApiController]
    [Route("api/login")]
    public class LoginKontrollera : ControllerBase
    {
        private readonly ErabiltzaileaRepository _repo;

        public LoginKontrollera(ErabiltzaileaRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var (success, error, user) = _repo.Login(dto.erabiltzailea, dto.pasahitza);

            if (!success)
            {
                return Unauthorized(new ErantzunaDTO<string>
                {
                    Code = 401,
                    Message = error,
                    Datuak = null
                });
            }

            var datuak = new
            {
                user.id,
                user.erabiltzailea,
                user.emaila,
                user.rola,
                user.txat
            };

            return Ok(new ErantzunaDTO<object>
            {
                Code = 200,
                Message = "Login egokia",
                Datuak = new List<object> { datuak }
            });
        }

        [HttpGet("{erabiltzaileId}/txat")]
        public IActionResult LortuTxatBaimena(int erabiltzaileId)
        {
            var (success, exists, error, txat) = _repo.LortuTxatBaimena(erabiltzaileId);

            if (!success)
            {
                return StatusCode(500, new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = error
                });
            }

            if (!exists)
            {
                return NotFound(new ErantzunaDTO<string>
                {
                    Code = 404,
                    Message = "Erabiltzailea ez da aurkitu"
                });
            }

            return Ok(new ErantzunaDTO<bool>
            {
                Code = 200,
                Message = "Txat baimena lortu da",
                Datuak = new List<bool> { txat }
            });
        }
    }
}
