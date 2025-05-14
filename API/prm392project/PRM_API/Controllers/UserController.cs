using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRM_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FoodDeliveryContext _foodDeliveryContext;
        public UserController (FoodDeliveryContext foodDeliveryContext)
        {
            _foodDeliveryContext = foodDeliveryContext;
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ThisIsAVeryStrongSecretKey12345!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [Authorize]
        [HttpPost("auth/local/logout")]
        public async Task<IActionResult> logout()
        {
            return Ok(true);
        }

        [HttpGet("auth/local/getCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // Lấy UserId từ token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                int userId = int.Parse(userIdClaim.Value);

                // Truy vấn user từ database
                var user = await _foodDeliveryContext.Users
                    .Where(u => u. UserId== userId)
                    .Select(u => new
                    {
                        UserID=u.UserId,
                        u.Username,
                        u.Email,
                        u.Address,
                        u.PhoneNumber,
                        u.Role
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpPost("auth/local/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest(new { message = "Email and Password are required" });
                }

                var user = await _foodDeliveryContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || user.Password != model.Password)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var token = GenerateJwtToken(user);

                // Trả về role của user
                return Ok(new
                {
                    access_token = token,
                    refresh_token = "SomeRefreshToken",
                    role = user.Role  // Thêm role vào response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }






        [HttpPost("auth/local/sigup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { message = "Invalid request data" });
                }
            
                var existingUser = await _foodDeliveryContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email is already registered" });
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Password = model.Password, 
                    Role = "User" 
                };

                _foodDeliveryContext.Users.Add(user);
                await _foodDeliveryContext.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                return Ok(new ResponseTokenDTO(token, "SomeRefreshToken"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

    }
}
