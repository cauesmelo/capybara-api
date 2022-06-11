using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace capybara_api.Services;

public class NoteService : BaseService {
    public NoteService(
        IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    AppDbContext context,
    UserManager<IdentityUser> userManager)
    : base(httpContextAccessor, configuration, context, userManager) { }

    public List<Note> Get() {
        List<Note> notes = context.note.AsNoTracking().OrderByDescending(d => d.updatedAt).ToList();
        return notes;
    }

    public Note Create(Note note) {
        context.note.Add(note);
        context.SaveChanges();
        return note;
    }

    public Note Update(Note note) {
        context.note.Update(note);
        context.SaveChanges();
        return note;
    }

    public void Delete(int id) {
        Note? note = context.note.Find(id);

        if(note == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        context.note.Remove(note);
        context.SaveChanges();
    }
}
