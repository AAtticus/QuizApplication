using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class QuizListViewModel
    {
        public String TeacherName { get; set; }
        public int Grade { get; set; }
        public String Subject { get; set; }
        public bool Exam { get; set; }
        public String StateName { get; set; }
        public int QuizID { get; set; }

        public QuizListViewModel(Quiz quiz)
        {
            this.TeacherName = quiz.Author.UserName;
            this.Grade = quiz.Grade;
            this.Subject = quiz.Subject;
            this.Exam = quiz.Exam;
            this.StateName = quiz.State.Name;
            this.QuizID = quiz.Id;
        }
    }
}