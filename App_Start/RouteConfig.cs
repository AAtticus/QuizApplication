using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuizApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TakeQuiz",
                url: "Student/TakeQuiz/{eid}/{index}",
                defaults: new { controller = "Student", action = "TakeQuiz", eid=1, index =0 }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddExerciseToQuiz",
                url: "{controller}/{action}/{qid}/{eid}",
                defaults: new { controller = "Quiz", action = "AddExerciseToQuiz"}
            );

            routes.MapRoute(
                name: "RemoveExerciseFromQuiz",
                url: "{controller}/{action}/{qid}/{eid}",
                defaults: new { controller = "Quiz", action = "RemoveExerciseFromQuiz" }
            );


        }
    }
}