using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace English_Vocabulary_Learning_Website.Models
{
    public class VocabularyContext : DbContext
    {
        public VocabularyContext() : base("name=VocabularyContext") { }

        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Result> Results { get; set; }

    }

    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "Admin" hoặc "User"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }

    public class Vocabulary
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string Pronunciation { get; set; }
        public string Example { get; set; }
        public virtual Topic Topic { get; set; }
    }

    public class Quiz
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; } // "A", "B", "C", "D"
        public virtual Topic Topic { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public System.DateTime CompletedAt { get; set; }
        public virtual UserAccount User { get; set; }
        public virtual Topic Topic { get; set; }
    }
   
}