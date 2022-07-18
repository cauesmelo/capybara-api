using capybara_api.Models.DTO;
using capybara_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase {
    private readonly LoginService loginService;

    public LoginController(LoginService loginService) {
        this.loginService = loginService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] Login loginData) {
        return Ok(loginService.Login(loginData));
    }
}
