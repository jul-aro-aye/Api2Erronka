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
    }
}
