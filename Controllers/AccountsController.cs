using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestProject.Entities;

namespace TestProject.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Check username/password from PostgreSQL here.

            if (request.Email != "admin@gmail.com" && request.Password != "Password@123")
                return Unauthorized();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return Ok(new { message = "Login successful" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Price = 1200,
                    Quantity = 10
                },
                new Product
                {
                    Id = 2,
                    Name = "Keyboard",
                    Price = 50,
                    Quantity = 25
                },
                new Product
                {
                    Id = 3,
                    Name = "Mouse",
                    Price = 25,
                    Quantity = 50
                },
                new Product
                {
                    Id = 4,
                    Name = "Monitor",
                    Price = 300,
                    Quantity = 15
                }
            };

            return Ok(products);
        }
    }
}
