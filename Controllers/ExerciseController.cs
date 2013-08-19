using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizApplication.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace QuizApplication.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ExerciseController : Controller
    {

        private QuizAppDb db = new QuizAppDb();

        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            PopulateCategoryDropdownList();
            return View(db.Exercises.ToList());
        }

        //
        // GET: /Exercise/Details/5

        public ActionResult Details(int id = 0)
        {
            Exercise exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            return View(exercise);
        }

        //
        // GET: /Exercise/Create

        public ActionResult Create()
        {
            PopulateCategoryDropdownList();
            var teacher = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            return View(new Exercise(teacher));
        }

        //
        // POST: /Exercise/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
             [Bind(Exclude = "Date")]
            Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                db.Exercises.Add(exercise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoryDropdownList(exercise.CategoryID);
            return View(exercise);
        }

        //
        // GET: /Exercise/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Exercise exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            PopulateCategoryDropdownList(exercise.CategoryID);
            return View(exercise);
        }

        //
        // POST: /Exercise/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Exclude = "Date")]
            Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exercise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoryDropdownList(exercise.CategoryID);
            return View(exercise);
        }

        //
        // GET: /Exercise/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Exercise exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            return View(exercise);
        }

        //
        // POST: /Exercise/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exercise exercise = db.Exercises.Find(id);
            db.Exercises.Remove(exercise);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateCategoryDropdownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Categories
                                   orderby d.Name
                                   select d;
            ViewBag.CategoryID = new SelectList(departmentsQuery, "Id", "Name", selectedDepartment);
        } 
    }
}