using capybara_api.Models.DTO;
using capybara_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace capybara_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoteController : ControllerBase {
    private readonly NoteService noteService;

    public NoteController(NoteService noteService) {
        this.noteService = noteService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(noteService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] NoteCreate note) {
        return Ok(noteService.Create(note));
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Put(int id, [FromBody] NoteUpdate note) {
        return Ok(noteService.Update(id, note));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(int id) {
        noteService.Delete(id);
        return Ok();
    }
}
