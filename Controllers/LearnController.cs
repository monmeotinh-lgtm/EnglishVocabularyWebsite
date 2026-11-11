using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using English_Vocabulary_Learning_Website.Models;

namespace English_Vocabulary_Learning_Website.Controllers
{
    public class LearnController : Controller
    {
        private VocabularyContext db = new VocabularyContext();

        // 1. Trang chủ học tập: Danh sách Chủ đề & Tiến độ học tập
        public ActionResult Index()
        {
            int currentUserId = Session["UserId"] != null ? (int)Session["UserId"] : 1;
            var topics = db.Topics.ToList();
            ViewBag.UserResults = db.Results.Where(r => r.UserId == currentUserId).ToList();
            return View(topics);
        }

        // 2. Màn hình học từ vựng qua Flashcard
        public ActionResult Flashcard(int topicId)
        {
            var topic = db.Topics.Find(topicId);
            if (topic == null) return HttpNotFound();

            ViewBag.Topic = topic;
            var vocabList = db.Vocabularies.Where(v => v.TopicId == topicId).ToList();
            return View(vocabList);
        }

        // 3. Màn hình làm bài trắc nghiệm (Quiz)
        public ActionResult TakeQuiz(int topicId)
        {
            var topic = db.Topics.Find(topicId);
            var questions = db.Quizzes.Where(q => q.TopicId == topicId).ToList();

            if (!questions.Any())
            {
                TempData["Message"] = "Chủ đề này chưa có bài trắc nghiệm!";
                return RedirectToAction("Index");
            }

            ViewBag.Topic = topic;
            return View(questions);
        }

        // 4. Xử lý chấm điểm Quiz và lưu kết quả vào Database
        [HttpPost]
        public ActionResult SubmitQuiz(QuizSubmissionViewModel model)
        {
            int currentUserId = Session["UserId"] != null ? (int)Session["UserId"] : 1;
            if (model == null || model.Answers == null) return RedirectToAction("Index");

            int score = 0;
            var quizIds = model.Answers.Select(a => a.QuizId).ToList();
            var dbQuizzes = db.Quizzes.Where(q => quizIds.Contains(q.Id)).ToDictionary(q => q.Id);

            foreach (var item in model.Answers)
            {
                if (dbQuizzes.ContainsKey(item.QuizId) &&
                    dbQuizzes[item.QuizId].CorrectAnswer.Trim().Equals(item.SelectedAnswer?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    score++;
                }
            }

            var result = new Result
            {
                UserId = currentUserId,
                TopicId = model.TopicId,
                Score = score,
                TotalQuestions = model.Answers.Count,
                CompletedAt = DateTime.Now
            };

            db.Results.Add(result);
            db.SaveChanges();

            return RedirectToAction("ResultDetails", new { id = result.Id });
        }

        // 5. Xem chi tiết kết quả bài làm
        public ActionResult ResultDetails(int id)
        {
            var result = db.Results.Include("Topic").FirstOrDefault(r => r.Id == id);
            if (result == null) return HttpNotFound();

            return View(result);
        }

        // 6. Tìm kiếm và phân trang từ vựng
        public ActionResult SearchVocabulary(string searchString, int? selectedTopicId, int page = 1, int pageSize = 6)
        {
            // Lấy danh sách chủ đề để đưa vào Dropdown bộ lọc
            ViewBag.Topics = db.Topics.ToList();

            var query = db.Vocabularies.AsQueryable();

            // 1. LINQ: Lọc theo từ khóa tìm kiếm (Từ vựng hoặc Nghĩa)
            if (!string.IsNullOrEmpty(searchString))
            {
                string keyword = searchString.Trim().ToLower();
                query = query.Where(v => v.Word.ToLower().Contains(keyword)
                                        || v.Meaning.ToLower().Contains(keyword));
            }

            // 2. LINQ: Lọc theo chủ đề (TopicId)
            if (selectedTopicId.HasValue && selectedTopicId.Value > 0)
            {
                query = query.Where(v => v.TopicId == selectedTopicId.Value);
            }

            // 3. LINQ: Sắp xếp theo ID trước khi phân trang
            query = query.OrderBy(v => v.Id);

            // Tính toán phân trang
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // 4. LINQ: Phân trang với Skip và Take
            var pagedList = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền dữ liệu trạng thái sang View
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentTopicId = selectedTopicId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedList);
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