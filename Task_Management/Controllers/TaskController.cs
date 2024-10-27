using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_Management.Hubs;
using Task_Management.Models;
using System.Security.Claims;

namespace Task_Management.Controllers
{
    public class TaskController : Controller
    {
        private TaskAppEntities db = new TaskAppEntities();

        // GET: Tasks
        [HttpGet]
        [Authorize]
        public ActionResult Index(string statusFilter, string taskNameFilter, string assignedToFilter)
        {
            var tasks = db.Tasks.AsQueryable();

            // Retrieve user role and ID from the session
            var userRole = Session["Role"]?.ToString();
            var userId = Convert.ToInt32(Session["UserId"]); 

            // Apply filters if provided
            if (!string.IsNullOrEmpty(statusFilter))
            {
                tasks = tasks.Where(t => t.Status == statusFilter);
            }

            if (!string.IsNullOrEmpty(taskNameFilter))
            {
                tasks = tasks.Where(t => t.Title.Contains(taskNameFilter));
            }

            if (!string.IsNullOrEmpty(assignedToFilter))
            {
                tasks = tasks.Where(t => t.AssignedTo.Contains(assignedToFilter));
            }

            if (userRole == "Admin")
            {
                ViewBag.Tasks = tasks.ToList();
            }
            else if (userRole == "User")
            {
                tasks = tasks.Where(t => t.CreatedBy == userId);
                ViewBag.Tasks = tasks.ToList();
            }
            else
            {
                ViewBag.Tasks = tasks.ToList();
            }
            return View();
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(task); 
        }


        public ActionResult Create()
        {
            ViewBag.Task = null;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTask(Task task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedBy = Convert.ToInt32(Session["UserId"]);
                task.CreatedAt = DateTime.Now;
                task.UpdatedAt = DateTime.Now;

                db.Tasks.Add(task);
                db.SaveChanges();

                NotificationHub.NotifyAllClients("A new task has been created: " + task.Title);

                return RedirectToAction("Index");
            }

            return View(task);
        }




        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        public ActionResult EditTask(Task task)
        {
            if (ModelState.IsValid)
            {
                var validStatuses = new List<string> { "Pending", "InProgress", "Completed" };
                if (!validStatuses.Contains(task.Status))
                {
                    ModelState.AddModelError("Status", "Invalid status value.");
                    return View(task);
                }

                var existingTask = db.Tasks.Find(task.TaskId);
                if (existingTask == null)
                {
                    return HttpNotFound();
                }
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Status = task.Status;
                existingTask.AssignedTo = task.AssignedTo;
                existingTask.UpdatedAt = DateTime.Now;

                try
                {
                    db.SaveChanges();
                    NotificationHub.NotifyAllClients($"Task '{task.Title}' has been updated.");
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the task: " + ex.InnerException?.Message);
                    return View(task);
                }
            }

            return View(task);
        }


        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.Task = task;
            return View();
        }

        // POST: Tasks/DeleteConfirmed/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task != null)
            {
                db.Tasks.Remove(task);
                db.SaveChanges();

                NotificationHub.NotifyAllClients($"Task '{task.Title}' has been deleted.");
            }
            return RedirectToAction("Index");
        }

    }
}