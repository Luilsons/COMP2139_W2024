using Microsoft.AspNetCore.Mvc;
using Lab1.Models;

namespace Lab1.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
             new Project {ProjectId = 1, Name = "Project 1", Description = "First Project" },
             new Project {ProjectId = 2, Name = "Project 2", Description = "Second Project" }
            // Feel free to add more projects
        };

            return View(projects);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = new Project { ProjectId = id, Name = "Project " + id, Description = "Details of Project " + id };

            return View(project);
        }


        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Project projects)
        {
            return RedirectToAction("Index");
        }
    }
}
