using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuizApplication.Models
{
   
    public class Exercise
    {

        public Exercise()
        {
            this.Date = DateTime.Now;
        }

        public Exercise(UserProfile user)
        {
            this.Date = DateTime.Now;
            this.Author = user;
            this.maxNbrOfSeconds = 0;
            this.maxNbrOfAttempts = 1;
        }

        public int Id{ get; set; }
        [Required]
        [Display(Name = "Question for this exercise")]
        public string Question { get; set; }
        [Required]
        [Display(Name = "Answer for this exercise")]
        public int Anwser { get; set; }
        [Required]
        [Display(Name = "Category for this exercise")]
        public int CategoryID { get; set; }
        [Range(1, 99)]
        [Required]
        [Display(Name = "Maximum Attempts")]
        public int maxNbrOfAttempts { get; set; }
        [Range(0, 1000)]
        [Required]
        [Display(Name = "Time in seconds (0 = unlimited)")]
        public int maxNbrOfSeconds { get; set; }
        [Display(Name = "Optional Hints")]
        public string Hints { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Attempt> Attempts { get; set; }

        public virtual DateTime Date { get; set; }        
        public virtual Category Category { get; set; }
        public virtual UserProfile Author { get; set; }

        }
}