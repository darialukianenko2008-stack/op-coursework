using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;
using Сoursework.Storage;

namespace Coursework_Tests
{
    public class SerializationTests
    {
        private SubjectRepo _subjectRepo;
        private QuestionRepo _questionRepo;
        private LogRepo _logRepo;

        [SetUp]
        public void Setup()
        {
            _subjectRepo = new SubjectRepo();
            _questionRepo = new QuestionRepo();
            _logRepo = new LogRepo();


            if (File.Exists(Path.Combine("Data", "subjects.json")))
                File.Delete(Path.Combine("Data", "subjects.json"));

            if (File.Exists(Path.Combine("Data", "questions.json")))
                File.Delete(Path.Combine("Data", "questions.json"));

            if (File.Exists(Path.Combine("Data", "logs.json")))
                File.Delete(Path.Combine("Data", "logs.json"));
        }

        [Test]
        public void SaveSubjects_ShouldCreateJsonFile()
        {
            Subject subject = new Subject(1, "Math");

            _subjectRepo.AddSubject(subject);

            Serialization.SaveSubjects(_subjectRepo);

            string path = Path.Combine("Data", "subjects.json");

            Assert.That(File.Exists(path), Is.True);
        }

        [Test]
        public void LoadSubjects_ShouldLoadSubjectsCorrectly()
        {
            Subject subject = new Subject(1, "Math");

            _subjectRepo.AddSubject(subject);

            Serialization.SaveSubjects(_subjectRepo);

            SubjectRepo newRepo = new SubjectRepo();

            Serialization.LoadSubjects(newRepo);

            Assert.That(newRepo.GetAll().Count, Is.EqualTo(1));
            Assert.That(newRepo.GetAll()[0].Name, Is.EqualTo("Math"));
        }

        [Test]
        public void SaveQuestions_ShouldCreateJsonFile()
        {
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");

            _questionRepo.AddQuestion(question);

            Serialization.SaveQuestions(_questionRepo);

            string path = Path.Combine("Data", "questions.json");

            Assert.That(File.Exists(path), Is.True);
        }

        [Test]
        public void LoadQuestions_ShouldLoadQuestionsCorrectly()
        {
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            _questionRepo.AddQuestion(question);
            Serialization.SaveQuestions(_questionRepo);
            QuestionRepo newRepo = new QuestionRepo();

            Serialization.LoadQuestions(newRepo);

            Assert.That(newRepo.GetAll().Count, Is.EqualTo(1));
            Assert.That(newRepo.GetAll()[0].TextOfQuestion, Is.EqualTo("What is c#"));
        }

        [Test]
        public void SaveLogs_ShouldCreateJsonFile()
        {
            Log log = new Log { Id = 1, SessionId = 1, QuestionId = 1, UserAnswer = "1", WasCorrect = true };
            _logRepo.GetAll().Add(log);

            Serialization.SaveLogs(_logRepo);
            string path = Path.Combine("Data", "logs.json");

            Assert.That(File.Exists(path), Is.True);
        }

        [Test]
        public void LoadLogs_ShouldLoadLogsCorrectly()
        {
            Log log = new Log { Id = 1, SessionId = 1, QuestionId = 1, UserAnswer = "1", WasCorrect = true };
            _logRepo.GetAll().Add(log);
            Serialization.SaveLogs(_logRepo);
            LogRepo newRepo = new LogRepo();

            Serialization.LoadLogs(newRepo);

            Assert.That(newRepo.GetAll().Count, Is.EqualTo(1));
            Assert.That(newRepo.GetAll()[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void SaveAll_ShouldCreateAllFiles()
        {
            _subjectRepo.AddSubject(new Subject(1, "Math"));
            _questionRepo.AddQuestion(new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4"));
            _logRepo.GetAll().Add(new Log { Id = 1, SessionId = 1, QuestionId = 1, UserAnswer = "1", WasCorrect = true });

            Serialization.SaveAll(_subjectRepo, _questionRepo, _logRepo);

            Assert.That(File.Exists(Path.Combine("Data", "subjects.json")), Is.True);
            Assert.That(File.Exists(Path.Combine("Data", "questions.json")), Is.True);
            Assert.That(File.Exists(Path.Combine("Data", "logs.json")), Is.True);
        }

        [Test]
        public void LoadAll_ShouldRestoreRelationsBetweenQuestionAndSubject()
        {
            Subject subject = new Subject(1, "Math");
            _subjectRepo.AddSubject(subject);
            OpenQuestion question = new OpenQuestion(1, new Subject(1, "OP"), Сoursework.Enums.DifficultyOfQuestion.Easy, "What is c#", "4");
            _questionRepo.AddQuestion(question);
            Serialization.SaveAll(_subjectRepo, _questionRepo, _logRepo);
            SubjectRepo loadedSubjects = new SubjectRepo();
            QuestionRepo loadedQuestions = new QuestionRepo();
            LogRepo loadedLogs = new LogRepo();

            Serialization.LoadAll(loadedSubjects, loadedQuestions, loadedLogs);

            Question loadedQuestion = loadedQuestions.GetAll().First();

            Assert.That(loadedQuestion.Topic, Is.Not.Null);
            Assert.That(loadedQuestion.Topic.Name, Is.EqualTo("Math"));
        }
    }
}