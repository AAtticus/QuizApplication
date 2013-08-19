using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int QuizID { get; set; }
        public int UserId { get; set; }
        public bool Completed { get; set; }

        public virtual DateTime Date { get; set; }
        public virtual UserProfile Student { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<Attempt> Attempts { get; set; }

        public Enrollment(Quiz quiz, UserProfile student)
        {
            this.Date = DateTime.Now;
            this.Quiz = quiz;
            this.Student = student;
            this.Completed = false;
        }

        public Enrollment()
        {
        }
    }
}