using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace capybara_api.Services;

public class NoteService : BaseService {
    public NoteService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager)
        : base(httpContextAccessor, configuration, context, userManager) { }

    public List<Note> Get() {
        string userId = GetUserId();

        List<Note> notes = context.note
            .Where(n => n.userId == userId)
            .AsNoTracking()
            .OrderByDescending(d => d.updatedAt)
            .ToList();

        return notes;
    }

    public Note Create(NoteCreate noteCreate) {
        string userId = GetUserId();

        Note note = new() { content = noteCreate.content, userId = userId };
        context.note.Add(note);
        context.SaveChanges();
        return note;
    }

    public Note Update(int id, NoteUpdate noteUpdate) {
        string userId = GetUserId();

        Note note = context.note.FirstOrDefault(n => n.id == id);

        if(note == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(note.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        note.content = noteUpdate.content;

        context.note.Update(note);
        context.SaveChanges();
        return note;
    }

    public void Delete(int id) {
        string userId = GetUserId();

        Note note = context.note.FirstOrDefault(n => n.id == id);

        if(note == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(note.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        context.note.Remove(note);
        context.SaveChanges();
    }

    private string GetUserId() {
        return httpContextAccessor.HttpContext
                        .User.Claims
                        .First(c => c.Type == ClaimTypes.NameIdentifier)
                        .Value;
    }
}
