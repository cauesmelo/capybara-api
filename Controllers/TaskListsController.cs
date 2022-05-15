using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capybara_api.Infra;
using capybara_api.Models;

namespace capybara_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskList()
        {
          if (_context.TaskList == null)
          {
              return NotFound();
          }
            return await _context.TaskList.ToListAsync();
        }

        // GET: api/TaskLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskList>> GetTaskList(Guid id)
        {
          if (_context.TaskList == null)
          {
              return NotFound();
          }
            var taskList = await _context.TaskList.FindAsync(id);

            if (taskList == null)
            {
                return NotFound();
            }

            return taskList;
        }

        // PUT: api/TaskLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskList(Guid id, TaskList taskList)
        {
            if (id != taskList.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskList>> PostTaskList(TaskList taskList)
        {
          if (_context.TaskList == null)
          {
              return Problem("Entity set 'ApplicationDbContext.TaskList'  is null.");
          }
            _context.TaskList.Add(taskList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskList", new { id = taskList.Id }, taskList);
        }

        // DELETE: api/TaskLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(Guid id)
        {
            if (_context.TaskList == null)
            {
                return NotFound();
            }
            var taskList = await _context.TaskList.FindAsync(id);
            if (taskList == null)
            {
                return NotFound();
            }

            _context.TaskList.Remove(taskList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskListExists(Guid id)
        {
            return (_context.TaskList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
