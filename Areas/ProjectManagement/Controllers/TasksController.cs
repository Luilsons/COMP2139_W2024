using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Application.Areas.ProjectManagement.Models;
using MVC_Application.Data;
using System.Transactions;

namespace MVC_Application.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext context)
        {
            _db = context;
        }

        // GET: Tasks
        [HttpGet("Index/{projectId:int}")]
        public async Task<IActionResult> Index(int projectId)
        {
            var tasks = await _db.ProjectTasks
                                .Where(t => t.ProjectId == projectId)
                                .ToListAsync();

            if (tasks.Count == 0)
            {
                ViewBag.AlertMessage = "There are no tasks associated with this project.";
            }

            ViewBag.ProjectId = projectId;
            return View(tasks);
        }


        // GET: Tasks/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskQuery = _db.ProjectTasks.AsQueryable();
            var task = await _db.ProjectTasks
                            .Include(t => t.Project)
                            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpGet("Create/{projectId:int}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = projectId
            };

            return View(task);
        }




        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.ProjectTasks
                                .Include(t => t.Project)
                                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.ProjectTasks
                    .Include(p => p.Project)
                    .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
        [HttpPost("DeleteConfirmed/{projectTaskId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int projectTaskId)
        {
            var task = await _db.ProjectTasks.FirstOrDefaultAsync(t => t.ProjectTaskId == projectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }
        //Lab 5 - Search ProjectTasks
        //Get: Tasks/Search/{searchString?}
        [HttpGet("Search/{searchString?}")]

        public async Task<IActionResult> Search(string searchString)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchString)
                                            || t.Description.Contains(searchString));
            }

            var tasks = await tasksQuery.ToListAsync();

            if (tasks.Any())
            {
                int firstTaskProjectId = tasks.First().ProjectId;

                return RedirectToAction(nameof(Index), new { projectId = firstTaskProjectId });
            }
            else
            {
                ViewBag.AlertMessage = "No tasks found with the provided search criteria.";
                return View("~/Areas/ProjectManagement/Views/Projects/Index");
            }
        }

    }
}
