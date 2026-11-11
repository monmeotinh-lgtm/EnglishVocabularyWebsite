using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using English_Vocabulary_Learning_Website.Models;

namespace English_Vocabulary_Learning_Website.Controllers
{
    public class AdminController : Controller
    {
        private VocabularyContext db = new VocabularyContext();

        // 1. Danh sách chủ đề (Đã tích hợp LINQ Tìm kiếm theo yêu cầu đồ án)
        public ActionResult Index(string searchString)
        {
            var topics = db.Topics.AsQueryable();

            // Dùng LINQ lọc kết quả nếu người dùng nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.Name.Contains(searchString) || t.Description.Contains(searchString));
            }

            ViewBag.SearchString = searchString; // Giữ lại từ khóa trên ô input
            return View(topics.ToList());
        }

        // 2. Thêm mới Chủ đề (Get)
        public ActionResult CreateTopic()
        {
            return View();
        }

        // 3. Thêm mới Chủ đề (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTopic(Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // 4. Chỉnh sửa Chủ đề (Get)
        public ActionResult EditTopic(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Topic topic = db.Topics.Find(id);
            if (topic == null) return HttpNotFound();
            return View(topic);
        }

        // 5. Chỉnh sửa Chủ đề (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTopic(Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // 6. Xóa Chủ đề (Đã xử lý điều kiện biên: Kiểm tra xem chủ đề có chứa từ vựng/câu hỏi không trước khi xóa)
        public ActionResult DeleteTopic(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Topic topic = db.Topics.Find(id);
            if (topic == null) return HttpNotFound();

            // Kiểm tra điều kiện biên: Nếu chủ đề đang có từ vựng hoặc câu hỏi thì không cho xóa trực tiếp (tránh lỗi khóa ngoại FK)
            bool hasVocab = db.Vocabularies.Any(v => v.TopicId == id);
            bool hasQuiz = db.Quizzes.Any(q => q.TopicId == id);

            if (hasVocab || hasQuiz)
            {
                TempData["Error"] = "Không thể xóa chủ đề này vì đang chứa từ vựng hoặc câu hỏi liên quan!";
                return RedirectToAction("Index");
            }

            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        // GET: /Admin/VocabularyList?topicId=1
        public ActionResult VocabularyList(int topicId)
        {
            var topic = db.Topics.Find(topicId);
            if (topic == null) return HttpNotFound();

            ViewBag.TopicName = topic.Name;
            ViewBag.TopicId = topicId;

            var vocabularies = db.Vocabularies.Where(v => v.TopicId == topicId).ToList();
            return View(vocabularies);
        }
        // 7. Thêm từ vựng mới cho chủ đề (Get)
        [HttpGet]
        public ActionResult AddVocabulary(int topicId)
        {
            ViewBag.TopicId = topicId;
            return View();
        }

        // 8. Thêm từ vựng mới cho chủ đề (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVocabulary(Vocabulary vocabulary)
        {
            if (ModelState.IsValid)
            {
                db.Vocabularies.Add(vocabulary);
                db.SaveChanges();
                return RedirectToAction("VocabularyList", new { topicId = vocabulary.TopicId });
            }
            ViewBag.TopicId = vocabulary.TopicId;
            return View(vocabulary);
        }

        // 9. Xóa từ vựng thuộc chủ đề
        public ActionResult DeleteVocabulary(int id)
        {
            var vocab = db.Vocabularies.Find(id);
            if (vocab == null) return HttpNotFound();

            int topicId = vocab.TopicId;
            db.Vocabularies.Remove(vocab);
            db.SaveChanges();

            return RedirectToAction("VocabularyList", new { topicId = topicId });
        }
        // 10. Xem danh sách câu hỏi trắc nghiệm thuộc chủ đề
        public ActionResult QuizList(int topicId)
        {
            var topic = db.Topics.Find(topicId);
            if (topic == null) return HttpNotFound();

            ViewBag.TopicName = topic.Name;
            ViewBag.TopicId = topicId;

            var quizzes = db.Quizzes.Where(q => q.TopicId == topicId).ToList();
            return View(quizzes);
        }

        // 11. Thêm câu hỏi trắc nghiệm mới (Get)
        [HttpGet]
        public ActionResult AddQuiz(int topicId)
        {
            ViewBag.TopicId = topicId;
            return View();
        }

        // 12. Thêm câu hỏi trắc nghiệm mới (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("QuizList", new { topicId = quiz.TopicId });
            }
            ViewBag.TopicId = quiz.TopicId;
            return View(quiz);
        }

        // 13. Xóa câu hỏi trắc nghiệm
        public ActionResult DeleteQuiz(int id)
        {
            var quiz = db.Quizzes.Find(id);
            if (quiz == null) return HttpNotFound();

            int topicId = quiz.TopicId;
            db.Quizzes.Remove(quiz);
            db.SaveChanges();

            return RedirectToAction("QuizList", new { topicId = topicId });
        }
    }
}