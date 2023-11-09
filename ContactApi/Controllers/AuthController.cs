using ContactApi.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository) => _authRepository = authRepository ?? 
            throw new ArgumentNullException(nameof(authRepository));
       
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO request)
        {
            try
            {
                var user = await _authRepository.Register(request);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO request)
        {
            try
            {
                var token = await _authRepository.Login(request);
                if (token != null)
                {
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized("Authentication failed");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


