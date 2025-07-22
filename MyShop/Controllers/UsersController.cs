
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;
//using service;
//using System.Text.Json;
//using Entity;
//using AutoMapper;
//using dto;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace MyShop.Controllers 
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class UsersController : ControllerBase
//    {

//        IUserService UserService;
//        IMapper _mapper;
//        ILogger<UsersController> _logger;
//        JwtTokenGenerator _jwtTokenGenerator;

//        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, IConfiguration config)
//        {
//            UserService = userService;
//            _mapper = mapper;
//            _logger = logger;
//            // קריאת ערכי JWT מהגדרות (appsettings.json)
//            var jwtSection = config.GetSection("Jwt");
//            _jwtTokenGenerator = new JwtTokenGenerator(
//                jwtSection["Key"]!,
//                jwtSection["Issuer"]!,
//                jwtSection["Audience"]!);
//        }

//        // GET api/<UsersController>/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<userDTO>> Get(int id)
//        {

//            User user = await UserService.GetUserById(id);
//            userDTO userDTO = _mapper.Map<User, userDTO>(user);
//            return userDTO == null ? BadRequest() : Ok(userDTO);
//        }

//        // POST api/<UsersController>
//        [AllowAnonymous] // הרשמה פתוחה לכולם
//        [HttpPost]
//        public async Task<ActionResult<object>> Register([FromBody] RegisterUserDTO user)
//        {
//            User newUser = _mapper.Map<RegisterUserDTO, User>(user);
//            User createdUser = await UserService.AddUser(newUser);
//            userDTO newUserDTO = _mapper.Map<User, userDTO>(createdUser);
//            if (newUserDTO == null)
//                return BadRequest();
//            var token = _jwtTokenGenerator.GenerateToken(createdUser);
//            Response.Cookies.Append("jwt", token, new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true, // רק ב-HTTPS
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTimeOffset.UtcNow.AddHours(2)
//            });
//            return Ok(new { user = newUserDTO });
//        }
//        [AllowAnonymous]
//        [HttpPost("password")]
//        public async Task<IActionResult> CheckPassword([FromBody] string password)
//        {
//            int Score = UserService.CheckPassword(password);
//            if (Score < 3)
//                return BadRequest(new { score = Score, message = "Password too weak" });
//            return Ok(new { score = Score });
//        }

//        [AllowAnonymous] 
//        [HttpPost("login")]
//        public async Task<ActionResult<object>> LogIn([FromQuery] string userName, string password)
//        {
//            _logger.LogCritical($"Login attempted with User name, {userName} and password {password}");
//            User userLogin = await UserService.LogIn(userName, password);
//            userDTO UserDTO = _mapper.Map<User, userDTO>(userLogin);
//            if (UserDTO == null)
//                return BadRequest();
//            var token = _jwtTokenGenerator.GenerateToken(userLogin);
//            Response.Cookies.Append("jwt", token, new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true,
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTimeOffset.UtcNow.AddHours(2)
//            });
//            return Ok(new { user = UserDTO });
//        }
//        // PUT api/<UsersController>/5
//        [HttpPut("{id}")]
//        public async Task<ActionResult<User>> Put(int id, [FromBody] RegisterUserDTO value)
//        {
//            User user = _mapper.Map<RegisterUserDTO, User>(value);
//            User newUser = await UserService.UpdateUser(id, user);
//            return newUser == null ? BadRequest(user) : Ok(newUser);

//        }

//        [HttpPost("logout")]
//        [AllowAnonymous]
//        public IActionResult Logout()
//        {
//            Response.Cookies.Delete("jwt");
//            return Ok();
//        }
//   }
//}

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
        public IActionResult CheckPassword([FromBody] string password)
        {
            int score = _userService.CheckPassword(password);
            return Ok(new { score });
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
