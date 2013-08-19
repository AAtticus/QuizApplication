using QuizApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace QuizApplication.Controllers
{
    [Authorize(Roles = "Student")]
    public class ReportController : Controller
    {

        private QuizAppDb db = new QuizAppDb();
        //
        // GET: /Report/

        public ActionResult Index(int eid)
        {
            Enrollment enrollment = db.Enrollments.Find(eid);

            if(!hasAccess(enrollment))
            {
                return RedirectToAction("Index", "Student", null);
            }

            if (!enrollment.Completed)
            {
                TempData["Message"] = "You haven't finished this quiz so we can't calculate a score card";
                TempData["MessageClass"] = "error";
                return RedirectToAction("Index", "Student", null);
            }

            List<Attempt> attempts = enrollment.Attempts.ToList();
            ICollection<FeedbackAttemptViewModel> allFeedbackView = new List<FeedbackAttemptViewModel>();

            foreach (Attempt attempt in attempts)
            {
                allFeedbackView.Add(new FeedbackAttemptViewModel(attempt));
            }

            ViewBag.Subject = enrollment.Quiz.Subject;
            ViewBag.Date = enrollment.Date;

            ViewBag.OnTen = FeedbackAttemptViewModel.CalculateTotalPointsOnTen(allFeedbackView);

            return View(allFeedbackView);
        }

        private bool hasAccess(Enrollment enrollment)
        {
            bool hasAccess = true;
            var student = db.UserProfiles.Find(WebSecurity.CurrentUserId);

            if (!student.Equals(enrollment.Student))
            {
                TempData["Message"] = "This is not your score card";
                TempData["MessageClass"] = "error";
                hasAccess = false;
            }

            return hasAccess;

        }

    }
}
