using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class ExerciseAttemptViewModel
    {
        public String Question { get; set; }
        public int MaxNbrOfSeconds { get; set; }
        public int MaxNbrOfAttempts { get; set; }
        public String Hints { get; set; }
        public int Answer { get; set; }

        public ExerciseAttemptViewModel(Exercise exercise)
        {
            this.Question = exercise.Question;
            this.MaxNbrOfSeconds = exercise.maxNbrOfSeconds;
            this.MaxNbrOfAttempts = exercise.maxNbrOfAttempts;
            this.Hints = exercise.Hints;

        }

        public ExerciseAttemptViewModel()
        {
        }
    }
}