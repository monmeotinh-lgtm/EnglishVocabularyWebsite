using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace English_Vocabulary_Learning_Website.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}