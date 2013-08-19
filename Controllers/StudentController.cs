using QuizApplication.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace QuizApplication.Controllers
{
    public class StudentController : Controller
    {

        private QuizAppDb db = new QuizAppDb();
    
        //
        // GET: /Student/

        public ActionResult Index()
        {
            UserProfile student = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            getAvailableQuizzes(student);
            getEnrollments(student);
            return View();
        }

        private void getAvailableQuizzes(UserProfile user)
        {
            var quizQuery = from state in db.States
                            join quiz in db.Quizzes on state equals quiz.State 
                            where quiz.Grade == user.Grade && (state.Name.Equals("Opengesteld") || state.Name.Equals("Laatste Kans"))
                            select quiz;

            ViewBag.AvailableQuizzes = quizQuery;

        }

        private void getEnrollments(UserProfile user)
        {
            var eQuery = from enrollment in db.Enrollments
                         where enrollment.UserId == user.UserId
                         select enrollment;

            ViewBag.Enrollments = eQuery;

        }

        private IQueryable<Quiz> getAvailableQuizzesInCollection(UserProfile user)
        {
            var quizQuery = from state in db.States
                            join quiz in db.Quizzes on state equals quiz.State
                            where quiz.Grade == user.Grade && (state.Name.Equals("Opengesteld") || state.Name.Equals("Laatste Kans"))
                            select quiz;

            return quizQuery;

        }

        private IQueryable<Enrollment> getOtherEnrollmentsForQuizAndUser(UserProfile user, Quiz quiz)
        {
            var eQuery = from enrollment in db.Enrollments
                         where enrollment.Student == user && enrollment.Quiz == quiz
                         select enrollment;

            return eQuery;

        }


        public ActionResult StartQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);

            if (quiz == null)
            {
                return RedirectToAction("Index");
            }

            UserProfile student = db.UserProfiles.Find(WebSecurity.CurrentUserId);

            if(quiz.Grade != student.Grade)
            {
                return RedirectToAction("Index");
            }

            if (quiz.Exam && getOtherEnrollmentsForQuizAndUser(student, quiz).Count() > 0)
            {
                return RedirectToAction("Index");
            }

            Enrollment enrollment = new Enrollment(quiz, student);
            db.Enrollments.Add(enrollment);
            db.SaveChanges();

            return RedirectToAction("TakeQuiz", new { id = enrollment.Id, index = 0 });

        }

        [HttpGet]
        public ActionResult TakeQuiz(int eid, int index)
        {
            Enrollment enrollment = db.Enrollments.Find(eid);

            if (enrollment == null || enrollment.Completed)
            {
                return RedirectToAction("Index");
            }

            Quiz quiz = enrollment.Quiz;
            ICollection<Exercise> exercises = quiz.Exercises;

            if (index > exercises.Count())
            {
                enrollment.Completed = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IEnumerator en = exercises.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                en.MoveNext();
            }

            Exercise exercise = (Exercise) en.Current;

            ExerciseAttemptViewModel eavm = new ExerciseAttemptViewModel(exercise);

            return View(eavm);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TakeQuiz(int eid, int index, int answer)
        {
            Enrollment enrollment = db.Enrollments.Find(eid);

            if (enrollment == null || enrollment.Completed)
            {
                return RedirectToAction("Index");
            }

            Quiz quiz = enrollment.Quiz;
            ICollection<Exercise> exercises = quiz.Exercises;

            if (index > exercises.Count())
            {
                enrollment.Completed = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            IEnumerator en = exercises.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                en.MoveNext();
            }

            Exercise exercise = (Exercise)en.Current;

            
            var attempt =  (from a in db.Attempts
                            where a.EnrollmentID == enrollment.Id && a.ExerciseID == exercise.Id
                            select a).
                            FirstOrDefault();

            if (attempt == null)
            {
                attempt = new Attempt(enrollment, exercise);
                db.Attempts.Add(attempt);
            }


            attempt.nbrOfAttempts = attempt.nbrOfAttempts + 1;
            attempt.Answer = answer;

            db.SaveChanges();


            return RedirectToAction("TakeQuiz", new { eid = enrollment.Id, index = index });

        }

     


    }
}
