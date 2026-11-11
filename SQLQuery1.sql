-- 1. Tạo bảng lưu lịch sử làm bài trắc nghiệm (Quiz History)
CREATE TABLE QuizHistories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Score INT NOT NULL,
    TotalQuestions INT NOT NULL,
    TestDate DATETIME DEFAULT GETDATE()
);
GO

-- 2. Tạo bảng quản lý tiến độ học từ vựng của người dùng (Progress Tracking)
CREATE TABLE UserProgresses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    TopicId INT NOT NULL,
    CompletedWordsCount INT DEFAULT 0,
    IsCompleted BIT DEFAULT 0,
    LastUpdated DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE Questions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestionText NVARCHAR(500) NOT NULL,
    OptionA NVARCHAR(200) NOT NULL,
    OptionB NVARCHAR(200) NOT NULL,
    OptionC NVARCHAR(200) NOT NULL,
    OptionD NVARCHAR(200) NOT NULL,
    CorrectAnswer VARCHAR(1) NOT NULL -- Lưu đáp án đúng: 'a', 'b', 'c', hoặc 'd'
);

-- Thêm dữ liệu mẫu câu hỏi từ vựng
INSERT INTO Questions (QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES 
(N'Từ nào đồng nghĩa với từ "Happy"?', N'Sad', N'Joyful', N'Angry', N'Tired', 'b'),
(N'Nghĩa của từ "Vocabulary" là gì?', N'Từ vựng', N'Ngữ pháp', N'Phát âm', N'Đọc hiểu', 'a'),
(N'Chọn dạng đúng của động từ: "She usually ... to school by bike."', N'go', N'going', N'goes', N'gone', 'c');
-- 1. Tạo bảng Quizzes (Câu hỏi trắc nghiệm theo Topic)
CREATE TABLE Quizzes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TopicId INT NOT NULL,
    Question NVARCHAR(500) NOT NULL,
    OptionA NVARCHAR(200) NOT NULL,
    OptionB NVARCHAR(200) NOT NULL,
    OptionC NVARCHAR(200) NOT NULL,
    OptionD NVARCHAR(200) NOT NULL,
    CorrectAnswer VARCHAR(10) NOT NULL, -- Lưu "A", "B", "C", hoặc "D"
    FOREIGN KEY (TopicId) REFERENCES Topics(Id)
);
GO

-- 2. Tạo bảng Results (Lưu lịch sử và điểm số làm bài)
CREATE TABLE Results (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    TopicId INT NOT NULL,
    Score INT NOT NULL,
    TotalQuestions INT NOT NULL,
    CompletedAt DATETIME NOT NULL,
    FOREIGN KEY (TopicId) REFERENCES Topics(Id)
);
GO

-- Thêm dữ liệu mẫu vào bảng Quizzes (TopicId = 1)
INSERT INTO Quizzes (TopicId, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer) 
VALUES 
(1, N'Từ nào đồng nghĩa với từ "Happy"?', N'Sad', N'Joyful', N'Angry', N'Tired', 'B'),
(1, N'Nghĩa của từ "Vocabulary" là gì?', N'Từ vựng', N'Ngữ pháp', N'Phát âm', N'Đọc hiểu', 'A'),
(1, N'Chọn dạng đúng của động từ: "She usually ... to school by bike."', N'go', N'going', N'goes', N'gone', 'C');
INSERT INTO Results (UserId, TopicId, Score, TotalQuestions, CompletedAt) 
VALUES (1, 1, 3, 3, GETDATE());
INSERT INTO Vocabularies (TopicId, Word, Meaning, Pronunciation, Example) 
VALUES 
