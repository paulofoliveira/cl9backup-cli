using Cl9Backup.CLI.FakeApi.Models;
using Cl9Backup.CLI.FakeApi.Persistence;
using Cl9Backup.CLI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Cl9Backup.CLI.FakeApi.Controllers
{
    // TODO: Criar filtro de autorização que valida a SessionKey.

    [Route("user/web")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("get-user-profile-and-hash", Name = "GetProfileAndHash")]
        public IActionResult GetProfileAndHash([FromForm] AuthModel model, [FromServices] UserProfileRepository repository) => Ok(repository.GetUserProfile(model.UserName));

        [HttpPost("dispatcher/run-backup-custom", Name = "RunBackup")]
        public async Task<IActionResult> RunBackup([FromForm] RunBackupRequestDto request)
        {
            await Task.Delay(1000);
            return Ok(new RunBackupResponseDto() { Status = 200, Message = "Successfully dispatched the instruction." });
        }
    }
}

