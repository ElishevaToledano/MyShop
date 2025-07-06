
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using service;
using System.Text.Json;
using Entity;
using AutoMapper;
using dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        IUserService UserService;
        IMapper _mapper;
        ILogger<UsersController> _logger;
        JwtTokenGenerator _jwtTokenGenerator;

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger, IConfiguration config)
        {
            UserService = userService;
            _mapper = mapper;
            _logger = logger;
            // קריאת ערכי JWT מהגדרות (appsettings.json)
            var jwtSection = config.GetSection("Jwt");
            _jwtTokenGenerator = new JwtTokenGenerator(
                jwtSection["Key"]!,
                jwtSection["Issuer"]!,
                jwtSection["Audience"]!);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<userDTO>> Get(int id)
        {

            User user = await UserService.GetUserById(id);
            userDTO userDTO = _mapper.Map<User, userDTO>(user);
            return userDTO == null ? BadRequest() : Ok(userDTO);
        }

        // POST api/<UsersController>
        [AllowAnonymous] // הרשמה פתוחה לכולם
        [HttpPost]
        public async Task<ActionResult<object>> Register([FromBody] RegisterUserDTO user)
        {
            User newUser = _mapper.Map<RegisterUserDTO, User>(user);
            User createdUser = await UserService.AddUser(newUser);
            userDTO newUserDTO = _mapper.Map<User, userDTO>(createdUser);
            if (newUserDTO == null)
                return BadRequest();
            var token = _jwtTokenGenerator.GenerateToken(createdUser);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // רק ב-HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });
            return Ok(new { user = newUserDTO });
        }

        [HttpPost("password")]
        public async Task<IActionResult> CheckPassword([FromBody] string password)
        {
            int Score = UserService.CheckPassword(password);
            return (Score < 3) ? BadRequest(Score) : Ok(Score);
        }
        //
        [AllowAnonymous] // כניסה פתוחה לכולם
        [HttpPost("login")]
        public async Task<ActionResult<object>> LogIn([FromQuery] string userName, string password)
        {
            _logger.LogCritical($"Login attempted with User name, {userName} and password {password}");
            User userLogin = await UserService.LogIn(userName, password);
            userDTO UserDTO = _mapper.Map<User, userDTO>(userLogin);
            if (UserDTO == null)
                return BadRequest();
            var token = _jwtTokenGenerator.GenerateToken(userLogin);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // רק ב-HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });
            return Ok(new { user = UserDTO }); // לא מחזירים את הטוקן בגוף
        }
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] RegisterUserDTO value)
        {
            User user = _mapper.Map<RegisterUserDTO, User>(value);
            User newUser = await UserService.UpdateUser(id, user);
            return newUser == null ? BadRequest(user) : Ok(newUser);

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
