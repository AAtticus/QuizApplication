using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class ExerciseListViewModel
    {
        public String Question;
        public int Answer;
        public DateTime Date;
        public String TeacherName;
        public String Category;
        public int ExerciseID;

        public ExerciseListViewModel(Exercise exercise)
        {
            this.Question = exercise.Question;
            this.Answer = exercise.Anwser;
            this.Date = exercise.Date;
            this.TeacherName = exercise.Author.ToString();
            this.Category = exercise.Category.ToString();
            this.ExerciseID = exercise.Id;
        }
    }
}