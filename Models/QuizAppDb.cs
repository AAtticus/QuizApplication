using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuizApplication.Models
{
    public class QuizAppDb : DbContext 
    {

        public QuizAppDb() : base("name=DefaultConnection")
        {
        
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
    }
}