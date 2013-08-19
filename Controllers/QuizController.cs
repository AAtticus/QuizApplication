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
            List<Quiz> allQuizzes = db.Quizzes.Include(a => a.Author).Include(s => s.State).ToList();
            ICollection<QuizListViewModel> allQuizzesView = new List<QuizListViewModel>();

            foreach (Quiz quiz in allQuizzes)
            {
                allQuizzesView.Add(new QuizListViewModel(quiz));
            }

            return View(allQuizzesView);

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

                var teacher = db.UserProfiles.Find(WebSecurity.CurrentUserId);
                quiz.Author = teacher;
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                TempData["Message"] = "Quiz succesfully created, you can now start adding questions";
                TempData["MessageClass"] = "success";
                return RedirectToAction("Details", new { id = quiz.Id } );
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

            if (!hasAccess(quiz))
            {
                return RedirectToAction("Index");
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

            if (!hasAccess(quiz))
            {
                return RedirectToAction("Index");
            }

            if (quiz == null)
            {
                return HttpNotFound();
            }

            if (quiz.StateID.Equals(1) || quiz.StateID.Equals(2))
            {
                return View(quiz);

            }

            TempData["Message"] = "You cannot delete a quiz which is not under construction or ready";
            TempData["MessageClass"] = "error";
            return RedirectToAction("Details", new { id = quiz.Id });

            
        }

        //
        // POST: /Quiz/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            
            Quiz quiz = db.Quizzes.Find(id);

            if (!hasAccess(quiz))
            {
                return RedirectToAction("Index");
            }

            if (quiz.StateID.Equals(1) || quiz.StateID.Equals(2))
            {
                db.Quizzes.Remove(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
               
            }

            TempData["Message"] = "You cannot delete a quiz which is not under construction or ready";
            TempData["MessageClass"] = "error";
            return RedirectToAction("Details", new { id = quiz.Id });

            
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

            if (!hasAccess(quiz))
            {
                return RedirectToAction("Index");
            }

            if (quiz == null || exercise == null)
            {
                return RedirectToAction("Index");
            }

            if (!quiz.StateID.Equals(1))
            {
                TempData["Message"] = "You cannot change a quiz which is not under construction";
                TempData["MessageClass"] = "error";
                return RedirectToAction("Details", new { id = quiz.Id });
            }

            if (getEnrollmentsForQuiz(quiz).Count() > 0)
            {
                TempData["Message"] = "There are already students taking this course";
                TempData["MessageClass"] = "error";
                return RedirectToAction("Details", new { id = quiz.Id });
            }

            quiz.Exercises.Add(exercise);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = quiz.Id });
        }

        public ActionResult RemoveExerciseFromQuiz(int qid, int eid)
        {
            Quiz quiz = db.Quizzes.Find(qid);
            Exercise exercise = db.Exercises.Find(eid);

            if (!hasAccess(quiz))
            {
                return RedirectToAction("Index");
            }

            if (quiz == null || exercise == null)
            {
                return RedirectToAction("Index");
            }

            if (!quiz.StateID.Equals(1))
            {
                TempData["Message"] = "You cannot change a quiz which is not under construction";
                TempData["MessageClass"] = "error";
                return RedirectToAction("Details", new { id = quiz.Id });
            }

            if (getEnrollmentsForQuiz(quiz).Count() > 0)
            {
                TempData["Message"] = "There are already students taking this course";
                TempData["MessageClass"] = "error";
                return RedirectToAction("Details", new { id = quiz.Id });
            }

            quiz.Exercises.Remove(exercise);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = quiz.Id });
        }

        private bool hasAccess(Quiz quiz)
        {
            bool hasAccess = true;
            var teacher = db.UserProfiles.Find(WebSecurity.CurrentUserId);

            if (!teacher.Equals(quiz.Author))
            {
                TempData["Message"] = "You don't have read/write access to this quiz";
                TempData["MessageClass"] = "error";
                hasAccess = false;
            }

            return hasAccess;

        }

        private IQueryable<Enrollment> getEnrollmentsForQuiz(Quiz quiz)
        {
            var eQuery = from enrollment in db.Enrollments
                         where enrollment.Quiz.Id == quiz.Id
                         select enrollment;

            return eQuery;
        }


    }
}