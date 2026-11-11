using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using English_Vocabulary_Learning_Website.Models;

namespace English_Vocabulary_Learning_Website.Models
{
    public class VocabularyInitializer : DropCreateDatabaseIfModelChanges<VocabularyContext>
    {
        protected override void Seed(VocabularyContext context)
        {
            // 1. Tạo tài khoản mẫu
            var user = new UserAccount { Username = "student1", Password = "123", Email = "sv@huflit.edu.vn", Role = "User" };
            var admin = new UserAccount { Username = "admin", Password = "123", Email = "admin@huflit.edu.vn", Role = "Admin" };
            context.Users.Add(user);
            context.Users.Add(admin);

            // 2. Tạo Chủ đề 1: Technology
            var topic1 = new Topic
            {
                Name = "Technology",
                Description = "Từ vựng về Công nghệ thông tin và Máy tính",
                ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=500"
            };

            // Tạo Chủ đề 2: Travel
            var topic2 = new Topic
            {
                Name = "Travel",
                Description = "Từ vựng về Du lịch và Khám phá",
                ImageUrl = "https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=500"
            };

            context.Topics.Add(topic1);
            context.Topics.Add(topic2);
            context.SaveChanges();

            // 3. Thêm Từ vựng cho Chủ đề 1 (Technology)
            context.Vocabularies.Add(new Vocabulary
            {
                TopicId = topic1.Id,
                Word = "Software",
                Meaning = "Phần mềm",
                Pronunciation = "/ˈsɒftweə/",
                Example = "ASP.NET is a software framework for building web apps."
            });
            context.Vocabularies.Add(new Vocabulary
            {
                TopicId = topic1.Id,
                Word = "Database",
                Meaning = "Cơ sở dữ liệu",
                Pronunciation = "/ˈdeɪtəbeɪs/",
                Example = "SQL Server is a relational database management system."
            });

            // Thêm Từ vựng cho Chủ đề 2 (Travel)
            context.Vocabularies.Add(new Vocabulary
            {
                TopicId = topic2.Id,
                Word = "Destination",
                Meaning = "Điểm đến",
                Pronunciation = "/ˌdestɪˈneɪʃn/",
                Example = "Da Lat is a popular tourist destination in Vietnam."
            });

            // 4. Thêm Câu hỏi Quiz cho Chủ đề 1 (Technology)
            context.Quizzes.Add(new Quiz
            {
                TopicId = topic1.Id,
                Question = "Từ 'Software' trong tiếng Việt có nghĩa là gì?",
                OptionA = "Phần cứng",
                OptionB = "Phần mềm",
                OptionC = "Mạng máy tính",
                OptionD = "Dữ liệu",
                CorrectAnswer = "B"
            });
            context.Quizzes.Add(new Quiz
            {
                TopicId = topic1.Id,
                Question = "'Database' được dịch sang tiếng Việt là gì?",
                OptionA = "Cơ sở dữ liệu",
                OptionB = "Máy chủ",
                OptionC = "Trình duyệt",
                OptionD = "Tập tin",
                CorrectAnswer = "A"
            });

            // Thêm Câu hỏi Quiz cho Chủ đề 2 (Travel)
            context.Quizzes.Add(new Quiz
            {
                TopicId = topic2.Id,
                Question = "Từ nào sau đây có nghĩa là 'Điểm đến'?",
                OptionA = "Luggage",
                OptionB = "Passport",
                OptionC = "Destination",
                OptionD = "Ticket",
                CorrectAnswer = "C"
            });

            context.SaveChanges();
            base.Seed(context);
        }
    }
}