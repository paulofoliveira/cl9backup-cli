using Cl9Backup.CLI.FakeApi.Models;
using Cl9Backup.CLI.FakeApi.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Cl9Backup.CLI.FakeApi.Controllers
{
    [Route("user/web")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("get-user-profile-and-hash", Name = "GetProfileAndHash")]
        public IActionResult GetProfileAndHash([FromForm] AuthModel model, [FromServices] UserProfileRepository repository) => Ok(repository.GetUserProfile(model.UserName));
    }
}
