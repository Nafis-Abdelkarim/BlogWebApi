using BlogWebApi.Models.ModelMapping;
using BlogWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAppAuthService _appAuthService;
        public UserController(IAppAuthService appAuthService)
        {
            _appAuthService = appAuthService;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(LoginUserDTO loginUser)
        {
            var token = await _appAuthService.Authenticate(loginUser);
            if(token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
