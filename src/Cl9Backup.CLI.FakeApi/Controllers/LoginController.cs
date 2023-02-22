using Cl9Backup.CLI.FakeApi.Models;
using Cl9Backup.CLI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cl9Backup.CLI.FakeApi.Controllers
{
    [Route("hybrid/session")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SessionKeyGenerator _keyGenerator;

        public LoginController(SessionKeyGenerator keyGenerator)
        {
            _keyGenerator = keyGenerator;
        }

        [HttpPost("start", Name = "PostLogin")]
        public IActionResult PostLogin([FromForm] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!request.UserName.Equals("cl9backup") || !request.Password.Equals("123"))
            {
                ModelState.AddModelError("UserName", "Usuário pode ser inválido");
                ModelState.AddModelError("Password", "Senha pode ser inválida");
                return BadRequest(ModelState);
            }

            if (!request.AuthType.Equals("Password"))
            {
                ModelState.AddModelError("AuthType", "AuthType deve ser do tipo Password");
                return BadRequest(ModelState);
            }

            var sessionKey = _keyGenerator.GenerateSessionKey();
            var response = new LoginResponseDto() { Status = 200, Message = "OK", SessionKey = sessionKey.SessionKey };
            return Ok(response);
        }
    }
}
