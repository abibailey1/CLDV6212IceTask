using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppointmentApp.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthenticationController(ApplicationDbContext context) {
            _context = context;
        }

        //Register new user
        [HttpPost("register")]
        public async Task<IActionResult> Register (RegistrationDTO dto) {
            //Create new user
            var newUser = new User { Userid = Guid.NewGuid().ToString(),
                                    FirstName = dto.FirstName,
                                    LastName = dto.LastName,
                                    Email = dto.Email,
                                    Role = dto.Role};

            //Hash user password
            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);

            //Store new user
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        //Logs an existing user in
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto) {
            //Find an existing user with the entered email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null) {
                return Unauthorized("Invalid email or password.");
            }

            //Verify the existing user's password
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed) {
                return Unauthorized("Invalid email or password.");
            }

            //Create cookie claims
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok("Logged in");
        }
    }
}
