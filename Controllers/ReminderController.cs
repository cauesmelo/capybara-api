using capybara_api.Models.DTO;
using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReminderController : ControllerBase {
    private readonly ReminderService reminderService;

    public ReminderController(ReminderService reminderService) {
        this.reminderService = reminderService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(reminderService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] ReminderCreate reminder) {
        return Ok(reminderService.Create(reminder));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id) {
        reminderService.Delete(id);
        return Ok();
    }
}
