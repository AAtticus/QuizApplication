using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizApplication.Models;
using WebMatrix.WebData;

namespace QuizApplication.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class QuizController : Controller
    {
        private QuizAppDb db = new QuizAppDb();

        //
        // GET: /Quiz/

        public ActionResult Index()
        {
            return View(db.Quizzes.ToList());
        }

        //
        // GET: /Quiz/Details/5

        public ActionResult Details(int id = 0)
        {

            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            ViewBag.AllEx = db.Exercises;

            return View(quiz);
        }

        //
        // GET: /Quiz/Create

        public ActionResult Create()
        {
            PopulateStateDropdownList();
            var teacher = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            return View(new Quiz(teacher));
        }

        //
        // POST: /Quiz/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Exclude=("StateID"))]
            Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateStateDropdownList(quiz.StateID);
            return View(quiz);
        }

        //
        // GET: /Quiz/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            PopulateStateDropdownList(quiz.StateID);
            return View(quiz);
        }

        //
        // POST: /Quiz/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateStateDropdownList(quiz.StateID);
            return View(quiz);
        }

        //
        // GET: /Quiz/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        //
        // POST: /Quiz/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            db.Quizzes.Remove(quiz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateStateDropdownList(object selectedState = null)
        {
            var stateQuery = from d in db.States
                                   orderby d.Name
                                   select d;
            ViewBag.StateID = new SelectList(stateQuery, "Id", "Name", selectedState);
        }

        public ActionResult AddExerciseToQuiz(int qid, int eid)
        {
            Quiz quiz = db.Quizzes.Find(qid);
            Exercise exercise = db.Exercises.Find(eid);

            if (quiz == null || exercise == null)
            {
                return RedirectToAction("Index");
            }

            quiz.Exercises.Add(exercise);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = quiz.Id });
        }

        public ActionResult RemoveExerciseFromQuiz(int qid, int eid)
        {
            Quiz quiz = db.Quizzes.Find(qid);
            Exercise exercise = db.Exercises.Find(eid);

            if (quiz == null || exercise == null)
            {
                return RedirectToAction("Index");
            }

            quiz.Exercises.Remove(exercise);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = quiz.Id });
        }
    }
}