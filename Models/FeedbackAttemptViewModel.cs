using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class FeedbackAttemptViewModel
    {
        public String question { get; set; }
        public int correctAnswer { get; set; }
        public int yourAnswer { get; set; }
        public int nbrOfAttempts { get; set; }
        public int MaxnbrOfAttempts { get; set; }
        public String Subject { get; set; }
        public DateTime Date { get; set; }

        public FeedbackAttemptViewModel(Attempt attempt)
        {
            this.question = attempt.Exercise.Question;
            this.correctAnswer = attempt.Exercise.Anwser;
            this.yourAnswer = attempt.Answer;
            this.nbrOfAttempts = attempt.nbrOfAttempts;
            this.MaxnbrOfAttempts = attempt.Exercise.maxNbrOfAttempts;
            this.Subject = attempt.Enrollment.Quiz.Subject;
            this.Date = attempt.Enrollment.Date;
        }

        public FeedbackAttemptViewModel()
        { 
        }

        public double CalculatePoints()
        {
            if (this.correctAnswer.Equals(this.yourAnswer) && this.nbrOfAttempts.Equals(1))
            {
                return 1.0;
            }

            if (this.correctAnswer.Equals(this.yourAnswer) && this.nbrOfAttempts > 1)
            {
                return 0.5;
            }

            return 0.0;
        }

        public static double CalculatePoints(FeedbackAttemptViewModel f)
        {
            if (f.correctAnswer.Equals(f.yourAnswer) && f.nbrOfAttempts.Equals(1))
            {
                return 1.0;
            }

            if (f.correctAnswer.Equals(f.yourAnswer) && f.nbrOfAttempts > 1)
            {
                return 0.5;
            }

            return 0.0;
        }


        public static double CalculateTotalPointsOnTen(ICollection<FeedbackAttemptViewModel> collection)
        {
            double total = 0;

            foreach(FeedbackAttemptViewModel attempt in collection)
            {
                total += FeedbackAttemptViewModel.CalculatePoints(attempt);
            }


            double onTen = (total * 10) / collection.Count();

            return onTen;
        }


    }

    
}