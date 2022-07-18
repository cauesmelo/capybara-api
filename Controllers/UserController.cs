using capybara_api.Models.DTO;
using capybara_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    private readonly UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserCreate user) {
        return Ok(userService.Create(user));
    }

    [HttpGet]
    public IActionResult Ping() {
        return Ok(new { response = "PONG!" });
    }
}
