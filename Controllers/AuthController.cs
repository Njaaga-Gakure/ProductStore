using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Model;
using ProductStore.Model.DTOs;
using ProductStore.Service.IService;
using ProductStore.Utils;
using System.Net;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _response;
        private readonly IJWT _jwt; 

        public AuthController(IUser userService, IMapper mapper, ResponseDTO responseDTO, IJWT jwt)
        {
            _userService = userService;
            _mapper = mapper;
            _response = responseDTO;
            _jwt = jwt; 

        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDTO>> Register(RegisterUserDTO registerUser)
        {
            try
            {
                var inputs = new List<string>()
                {
                    registerUser.Name,
                    registerUser.Email,
                    registerUser.Password
                };

                var isNullOrEmpty = ValidateInputs.Validate(inputs);
                if (isNullOrEmpty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Please fill in all the field :(";
                    return BadRequest(_response);
                }
                var existingUser = await _userService.GetUserByEmail(registerUser.Email);
                
                if (existingUser != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "User already exists. :(";
                    return BadRequest(_response);
                }

                var newUser = _mapper.Map<User>(registerUser);
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = await _userService.AddUser(newUser);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);

            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login(LoginUserDTO loginUser)
        {
            try
            {
                var inputs = new List<string>()
                {
                    loginUser.Email,
                    loginUser.Password
                };

                var isNullOrEmpty = ValidateInputs.Validate(inputs);
                if (isNullOrEmpty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Please fill in all the field :(";
                    return BadRequest(_response);
                }

                var existingUser = await _userService.GetUserByEmail(loginUser.Email);

                if (existingUser == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Invalid Credentials :(";
                    return BadRequest(_response);
                }

                var isValidPassword = BCrypt.Net.BCrypt.Verify(loginUser.Password, existingUser.Password);

                if (!isValidPassword)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Invalid Credentials :(";
                    return BadRequest(_response);
                }
                var token = _jwt.CreateJWTToken(existingUser);
                _response.Result = token;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ?  ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }

        }

    }
}
