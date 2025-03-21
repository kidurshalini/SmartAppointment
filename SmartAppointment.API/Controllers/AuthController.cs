﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartAppointment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // ✅ REGISTER ENDPOINT (Assigns Role)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign Role (if provided and valid)
                if (!string.IsNullOrEmpty(model.Role))
                {
                    if (await _userManager.IsInRoleAsync(user, model.Role) == false)
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }
                }

                return Ok(new { message = $"User Registered Successfully as {model.Role ?? "User"}!" });
            }

            return BadRequest(result.Errors);
        }

        //// ✅ LOGIN ENDPOINT (Generates JWT Token with Role)
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);

        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        var token = GenerateJwtToken(user, roles);

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }

        //    return Unauthorized("Invalid email or password.");
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, roles);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userId = user.Id,
                    role = roles.FirstOrDefault() // Return the first role
                });
            }

            return Unauthorized("Invalid email or password.");
        }


        // ✅ GENERATE JWT TOKEN WITH ROLE
        private JwtSecurityToken GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            // Create claims for the token
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id), // User ID
        new Claim(ClaimTypes.Email, user.Email),       // Email
        new Claim(ClaimTypes.Name, user.UserName)      // Username
    };

            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // Token expiration
                signingCredentials: credentials
            );

            return token;
        }


        // ✅ LOGOUT ENDPOINT
        [HttpPost("logout")]
        [Authorize] // Ensure only authenticated users can access this endpoint
        public IActionResult Logout()
        {
            // Optionally, you can blacklist the token here if needed
            // For now, just return a success response
            return Ok(new { message = "Logout successful" });
        }
    }

    // ✅ Models
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin, Professional, User
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}