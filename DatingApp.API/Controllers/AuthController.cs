using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserForRegisterDto userForRegisterDto){
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); 
            if(await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User already exists"); 
            
            var user = new User{UserName = userForRegisterDto.Username}; 
            var createdUser = await _repo.Register(user, userForRegisterDto.Password); 
            return StatusCode(201); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto){
            userForLoginDto.Username = userForLoginDto.Username.ToLower(); 
            User user = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password); 
            if(user == null)
                return Unauthorized();

            //claims are information that will be part of a token's payload
            //section
            var claims = new []{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Name, user.UserName), 
                new Claim(ClaimTypes.Surname, "Singh")
            };

            //we also need a key to sign our token and this part is going to be 
            //hashed. Server validates a token using this security key - If the token is 
            //actually the one validated and created by server who is sending request back.
            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //token descriptor that is going to contain expiry date of our token
            //and signing credentials 
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //return the token to client 
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });






        }
    }
}