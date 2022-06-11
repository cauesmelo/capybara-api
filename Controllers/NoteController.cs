using capybara_api.Models;
using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoteController : ControllerBase {
    private readonly NoteService noteService;

    public NoteController(NoteService disciplineService) {
        this.noteService = disciplineService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(noteService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Note note) {
        return Ok(noteService.Create(note));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Put([FromBody] Note note) {
        return Ok(noteService.Update(note));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id) {
        noteService.Delete(id);
        return Ok();
    }
}
