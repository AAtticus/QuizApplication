using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class Quiz
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Range(1, 6)]
        public int Grade { get; set; }
        public bool Exam { get; set; }
        public int StateID { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual UserProfile Author { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual State State{ get; set; }

        public Quiz()
        {
            this.Date = DateTime.Now;
            this.StateID = 1;
        }

        public Quiz(UserProfile author)
        {
            this.Author = author;
            this.Date = DateTime.Now;
            this.StateID = 1;
        }
    }
}