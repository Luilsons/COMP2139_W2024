using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Application.Data;
using MVC_Application.Models;

namespace MVC_Application.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext context)
        {
            _db = context;
        }

        // GET: Tasks
        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks
                                .Where(t => t.ProjectId == projectId)
                                .ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        // GET: Tasks/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _db.ProjectTasks
                            .Include(t => t.Project)
                            .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _db.ProjectTasks
                                .Include(t => t.Project)
                                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _db.ProjectTasks
                    .Include(p => p.Project)
                    .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskId)
        {
            var task = _db.ProjectTasks.FirstOrDefault(t => t.ProjectTaskId == projectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }


    }
}
