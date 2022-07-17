using capybara_api.Models;
using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskUnityController : ControllerBase {
    private readonly TaskUnityService taskUnityService;

    public TaskUnityController(TaskUnityService taskUnityService) {
        this.taskUnityService = taskUnityService;
    }

    [HttpPatch]
    [Route("{id}")]
    public IActionResult Patch([FromBody] TaskUnity task) {
        return Ok(taskUnityService.Update(task));
    }
}
