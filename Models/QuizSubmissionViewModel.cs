using System.Collections.Generic;

namespace English_Vocabulary_Learning_Website.Models
{
    public class QuizSubmissionViewModel
    {
        public int TopicId { get; set; }
        public List<QuizAnswerDto> Answers { get; set; }
    }

    public class QuizAnswerDto
    {
        public int QuizId { get; set; }
        public string SelectedAnswer { get; set; }
    }
}