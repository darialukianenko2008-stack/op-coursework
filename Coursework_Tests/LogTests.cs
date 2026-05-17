using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;
using Сoursework.Storage;

namespace Coursework_Tests
{
    public class LogTests
    {
        private LogRepo _logRepo;

        [SetUp]
        public void Setup()
        {
            _logRepo = new LogRepo();
        }

        [Test]
        public void SaveSessionResult_ShouldCreateLogs()
        {
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            Test test = new Test(1, new List<Question>  { question });
            _logRepo.SaveSessionResult(test, 1);

            List<Log> logs = _logRepo.GetAll();

            Assert.That(logs.Count, Is.EqualTo(1));
            Assert.That(logs[0].SessionId, Is.EqualTo(1));
        }

        [Test]
        public void SaveSessionResult_ShouldIncreaseQuestionStatistics()
        {
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            Test test = new Test(1, new List<Question> { question });

            _logRepo.SaveSessionResult(test, 1);

            Assert.That(question.TotalAttempts, Is.EqualTo(1));
            Assert.That(question.CorrectAttempts, Is.EqualTo(0));
        }

        [Test]
        public void SaveSessionResult_ShouldHandleMissingAnswer()
        {
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            Test test = new Test(1, new List<Question> { question });

            _logRepo.SaveSessionResult(test, 1);
            Log log = _logRepo.GetAll().First();

            Assert.That(log.UserAnswer, Is.EqualTo("No answer given."));
            Assert.That(log.WasCorrect, Is.False);
        }

        [Test]
        public void GetWrongAnswersBySession_ShouldReturnOnlyWrongAnswers()
        {
            OpenQuestion question1 = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            OpenQuestion question2 = new OpenQuestion(2, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c++", "4");
            Test test = new Test(1, new List<Question> { question1, question2 });
            test.UserAnswers[1] = "4";
            _logRepo.SaveSessionResult(test, 10);

            List<Log> wrongAnswers = _logRepo.GetWrongAnswersBySession(10);

            Assert.That(wrongAnswers.Count, Is.EqualTo(1));
            Assert.That(wrongAnswers[0].QuestionId, Is.EqualTo(2));
        }

        [Test]
        public void GetSessionScore_ShouldReturnCorrectScore()
        {
            OpenQuestion question1 = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            OpenQuestion question2 = new OpenQuestion(2, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c++", "4");
            Test test = new Test(1, new List<Question> { question1, question2 });
            _logRepo.SaveSessionResult(test, 10);

            (int correct, int total) = _logRepo.GetSessionScore(10);

            Assert.That(correct, Is.EqualTo(0));
            Assert.That(total, Is.EqualTo(2));
        }

        [Test]
        public void GetAll_ShouldReturnAllLogs()
        {
            OpenQuestion question1 = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            Test test = new Test(1, new List<Question> { question1 });
            _logRepo.SaveSessionResult(test, 1);

            List<Log> logs = _logRepo.GetAll();

            Assert.That(logs, Is.Not.Null);
            Assert.That(logs.Count, Is.EqualTo(1));
        }

        [Test]
        public void PrintAllHistory_ShouldPrintSessionStatistics()
        {
            OpenQuestion question1 = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            Test test = new Test(1, new List<Question> { question1 });
            test.UserAnswers[1] = "4";
            _logRepo.SaveSessionResult(test, 1);

            (int correct, int total) = _logRepo.GetSessionScore(1);

            double percentage = Math.Round((double)correct / total * 100, 1);

            Assert.That(correct, Is.EqualTo(1));
            Assert.That(total, Is.EqualTo(1));
            Assert.That(percentage, Is.EqualTo(100.0));
        }
    }
}