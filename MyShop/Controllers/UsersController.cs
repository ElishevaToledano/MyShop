using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using service;
using AutoMapper;
using Entity;
using dto;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, IConfiguration config)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;

            var jwtConfig = config.GetSection("Jwt");
            _jwtTokenGenerator = new JwtTokenGenerator(
                jwtConfig["Key"]!,
                jwtConfig["Issuer"]!,
                jwtConfig["Audience"]!);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<userDTO>> Get(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            var dto = _mapper.Map<userDTO>(user);
            return Ok(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Register([FromBody] RegisterUserDTO user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Password) ||
                    string.IsNullOrWhiteSpace(user.UserName))
                {
                    return BadRequest("Username and password are required.");
                }
                var newUser = _mapper.Map<User>(user);
                var createdUser = await _userService.AddUser(newUser);
                if (createdUser == null)
                    return BadRequest("Password too weak or registration failed");
                var dto = _mapper.Map<userDTO>(createdUser);
                var token = _jwtTokenGenerator.GenerateToken(createdUser);
                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(2)
                });
                return Ok(new { user = dto });
            }
            catch (Repository.Exceptions.UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed: " + ex.Message);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> LogIn([FromQuery] string userName, string password)
        {
            _logger.LogInformation($"Login attempt for user: {userName}");

            var user = await _userService.LogIn(userName, password);
            if (user == null)
                return NoContent();
            var dto = _mapper.Map<userDTO>(user);
            var token = _jwtTokenGenerator.GenerateToken(user);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });
            return Ok(new { user = dto });
        }

        [HttpPost("password")]
        [AllowAnonymous]
        public IActionResult CheckPassword([FromBody] PasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Password is required");
            }
            
            try 
            {
                int score = _userService.CheckPassword(request.Password);
                return Ok(new { score });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking password strength");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        
        public class PasswordRequest
        {
            public string Password { get; set; } = string.Empty;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] RegisterUserDTO value)
        {
            var user = _mapper.Map<User>(value);
            var updatedUser = await _userService.UpdateUser(id, user);
            if (updatedUser == null)
                return BadRequest("Password too weak or update failed");

            return Ok(updatedUser);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }
    }
}
