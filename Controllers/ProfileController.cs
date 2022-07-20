using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

public class ProfileController : ControllerBase {
    private readonly ProfileService profileService;

    public ProfileController(ProfileService profileService) {
        this.profileService = profileService;
    }

    [HttpPatch]
    public IActionResult Post([FromBody] string email) {
        return Ok(profileService.UpdateEmail(email));
    }

    [HttpPut]
    public IActionResult Put([FromBody] string theme) {
        return Ok(profileService.UpdateTheme(theme));
    }
}
