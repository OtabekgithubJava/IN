using JobBoard.Domain.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Application.Abstractions.IService;
namespace JobBoard.API.Controllers.Indentity
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class AuthController : ControllerBase
    {
        private readonly IAuthSevice _authSevice;
        
        public AuthController(IAuthSevice authService)
        {
            _authSevice = authService;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<ResponseLogin>> Login([FromForm]RequestLogin model)
        {
            var result = await _authSevice.GenerateToken(model);

            return Ok(result);
        }
        
        [HttpPost("register")]
        public async Task<string> CreateUser([FromForm] UserDTO userDto)
        {
            var result = await _authSevice.CreateUser(userDto);

            return result;
        }
    }
}