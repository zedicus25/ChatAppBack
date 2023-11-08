using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;

namespace ChatAppApi.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorks _unitOfWorks;

        public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration, IUnitOfWorks unitOfWorks)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWorks = unitOfWorks;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };


                var token = GetToken(authClaims);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("regUser")]
        public async Task<IActionResult> RegUser([FromBody] Register model)
        {
            var userEx = await _userManager.FindByNameAsync(model.UserName);
            if (userEx != null) return StatusCode(StatusCodes.Status500InternalServerError, "User in db already");

            IdentityUser user = new()
            {
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded) { return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!"); }

            _unitOfWorks.UsersRepository.Add(new User()
            {
                Nickname = model.UserName,
                IdentityId = user.Id
            });

            return Ok("User added!");
        }

        private JwtSecurityToken GetToken(List<Claim> claimsList)
        {
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(6),
                    claims: claimsList,
                    signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}
