using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using English_Vocabulary_Learning_Website.Models;

namespace English_Vocabulary_Learning_Website.Controllers
{
    public class QuizController : Controller
    {
        private VocabularyContext db = new VocabularyContext();

        // GET: /Quiz/Index?topicId=1
        public ActionResult Index(int topicId = 1)
        {
            // Lấy danh sách câu hỏi thuộc chủ đề từ CSDL thông qua Entity Framework
            var questions = db.Quizzes.Where(q => q.TopicId == topicId).ToList();
            ViewBag.TopicId = topicId;
            return View(questions);
        }

        // POST: /Quiz/Submit
        [HttpPost]
        public ActionResult Submit(FormCollection form, int topicId = 1)
        {
            int score = 0;
            var questions = db.Quizzes.Where(q => q.TopicId == topicId).ToList();

            foreach (var q in questions)
            {
                string userAnswer = form["question_" + q.Id]; // Nhận giá trị "A", "B", "C", "D" từ View

                if (!string.IsNullOrEmpty(userAnswer) && userAnswer.Trim().ToUpper() == q.CorrectAnswer.Trim().ToUpper())
                {
                    score++;
                }
            }

            // Lấy UserId từ Session (nếu chưa đăng nhập tạm gán bằng 1)
            int userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;

            // Lưu kết quả vào bảng Results bằng Entity Framework
            var result = new Result
            {
                UserId = userId,
                TopicId = topicId,
                Score = score,
                TotalQuestions = questions.Count,
                CompletedAt = DateTime.Now
            };

            db.Results.Add(result);
            db.SaveChanges(); // Tự động ghi vào CSDL SQL Server

            TempData["Score"] = score;
            TempData["Total"] = questions.Count;

            return RedirectToAction("Result");
        }

        // GET: /Quiz/Result
        public ActionResult Result()
        {
            if (TempData["Score"] == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Score = TempData["Score"];
            ViewBag.Total = TempData["Total"];

            return View();
        }

        // GET: /Quiz/History (Xem lịch sử làm bài)
        public ActionResult History()
        {
            int userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1;

            // Lấy danh sách kết quả của user hiện tại, sắp xếp mới nhất lên đầu
            var histories = db.Results
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CompletedAt)
                .ToList();

            return View(histories); // Truyền model chuẩn sang View
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}