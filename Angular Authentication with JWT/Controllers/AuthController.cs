using Angular_Authentication_with_JWT.Dto_s;
using Angular_Authentication_with_JWT.JwtConfigration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Angular_Authentication_with_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly JwtConfig _jwtconfig;

        public AuthController(UserManager<IdentityUser> usermanager,IOptionsMonitor<JwtConfig> optionJwt)
        {
            _usermanager = usermanager;
            _jwtconfig = optionJwt.CurrentValue;
        }

        [HttpPost("Regstir")]
        public async Task<IActionResult> RegstirAsync([FromBody]UerRegsiterDto RequistDto)
        {
            if(ModelState.IsValid)
            {
                //chack Email Exixt
                var email=await _usermanager.FindByEmailAsync(RequistDto.Email);
                if (email != null)
                {
                    return BadRequest("Email Alredt Exixst");
                }
                var newUser = new IdentityUser()
                {
                    Email=RequistDto.Email,
                    UserName=RequistDto.Email
                };
                var isCreated=await _usermanager.CreateAsync(newUser,RequistDto.Password);

                if(isCreated.Succeeded)
                {
                    var token = GenreteJwtToken(newUser);
                  return Ok(new ResgserRespons() {
                  Result=true,
                  Token=token
                  });
                }
                return BadRequest("Inalid Requist Please try Again");

            }
            return BadRequest("Invaild Requist");
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult>LogInAsync(UserLogin dto)
        {
            if(ModelState.IsValid)
            {
                var User = await _usermanager.FindByEmailAsync(dto.Email);
                if(User is null)
                {
                    return BadRequest($"Not Found{User}");
                }

                var cheakpaswword = await _usermanager.CheckPasswordAsync(User, dto.Password);
                if(!cheakpaswword)
                {
                    return BadRequest("Not Vaild");
                }
                var token= GenreteJwtToken(User);
                return Ok(new LoginRespons {Token=token,Result=true });
            }
            return BadRequest("Not Vaild");
        }






        private string GenreteJwtToken(IdentityUser user)
        {
            var JwtTokenHandler=new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtconfig.Key);
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim("Id",user.Id),
                  new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                  new Claim(JwtRegisteredClaimNames.Email,user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };
            var token = JwtTokenHandler.CreateToken(tokenDescription);
            var JwtToken=JwtTokenHandler.WriteToken(token);
            return JwtToken;
        }
    }
}
