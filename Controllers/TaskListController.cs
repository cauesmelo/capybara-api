using capybara_api.Models;
using capybara_api.Models.DTO;
using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskListController : ControllerBase {
    private readonly TaskListService taskListService;

    public TaskListController(TaskListService taskListService) {
        this.taskListService = taskListService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(taskListService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] TaskListCreate taskList) {
        return Ok(taskListService.Create(taskList));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Put([FromBody] TaskList taskList) {
        return Ok(taskListService.Update(taskList));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id) {
        taskListService.Delete(id);
        return Ok();
    }
}
