using Microsoft.AspNetCore.Mvc;
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

    public class UsersController : ControllerBase
    {

        IUserService UserService;
        IMapper _mapper;
        ILogger<UsersController> _logger;
        
        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger)
        {
            UserService = userService;
              _mapper=mapper;
            _logger = logger;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<userDTO>> Get(int id) {

            User user = await UserService.GetUserById(id);
            userDTO userDTO = _mapper.Map<User, userDTO>(user);
        return userDTO == null? BadRequest() :Ok(userDTO);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<userDTO>> Register([FromBody] RegisterUserDTO user)
        {

            User newUser =  _mapper.Map<RegisterUserDTO, User>(user);
            User userDTO = await UserService.AddUser(newUser);
            userDTO newUserDTO = _mapper.Map<User, userDTO>(userDTO);
            return newUserDTO != null?  Ok(newUserDTO): BadRequest(newUserDTO);
        }
            [HttpPost("password")]
        public async Task<IActionResult> CheckPassword([FromBody] string password)
        {
          int Score =  UserService.CheckPassword(password);
          return  (Score < 3)?BadRequest(Score):Ok(Score);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LogIn([FromQuery] string userName , string password)
        {
                _logger.LogCritical($"Login attempted with User name, {userName} and password {password}");
                User userLogin = await UserService.LogIn(userName, password);
                userDTO UserDTO = _mapper.Map<User, userDTO>(userLogin);
                //return userDTO == null ? NoContent() : Ok(userDTO);
                return UserDTO == null ? BadRequest() : Ok(UserDTO);

        }
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] RegisterUserDTO value)
        {
            User user = _mapper.Map<RegisterUserDTO, User>(value);
            User newUser = await UserService.UpdateUser(id, user);
            return newUser == null ? BadRequest(user) : Ok(newUser);

        }
   }
}
