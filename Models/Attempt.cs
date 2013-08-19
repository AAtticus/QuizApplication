using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public int Answer { get; set; }
        public int nbrOfAttempts { get; set; }
        public int ExerciseID { get; set; }
        public int EnrollmentID { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual Enrollment Enrollment { get; set; }

        public Attempt(Enrollment enrollment, Exercise exercise)
        {
            this.Enrollment = enrollment;
            this.Exercise = exercise;
        }

        public Attempt() 
        {
        }
      
    }
}