using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;
using Сoursework.Storage;

namespace Coursework_Tests
{
    public class QuestionTests
    {
        private QuestionRepo _questionRepo;
        private Subject _subject;

        [SetUp]
        public void Setup()
        {
            _questionRepo = new QuestionRepo();
            _subject = new Subject(1, "Programming");
        }

        [Test]
        public void CreateSingleChoice_ShouldCreateSingleAnswerQuestion()
        {
            Question question = _questionRepo.CreateSingleChoice(
                _subject, 
                DifficultyOfQuestion.Easy, 
                "What is C#?", 
                "Language", 
                new List<string> 
                { 
                    "Language", 
                    "Browser" 
                }
            );

            Assert.That(question, Is.InstanceOf<SingleAnswerOption>());
            Assert.That(question.TextOfQuestion, Is.EqualTo("What is C#?"));
            Assert.That(question.Answer, Is.EqualTo("Language"));
        }

        [Test]
        public void CreateMultipleChoice_ShouldCreateMultiAnswerQuestion()
        {
            Question question = _questionRepo.CreateMultipleChoice(
                _subject,
                DifficultyOfQuestion.Normal,
                "Select OOP principles",
                "Encapsulation,Polymorphism",
                new List<string>
                {
                    "Encapsulation",
                    "Polymorphism",
                    "HTML"
                }
            );

            Assert.That(question, Is.InstanceOf<MultiAnswerOption>());
            Assert.That(question.TextOfQuestion, Does.Contain("OOP"));
        }

        [Test]
        public void CreateOpenQuestion_ShouldCreateOpenQuestion()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Hard,
                "What is .NET?",
                "Framework"
            );

            Assert.That(question, Is.InstanceOf<OpenQuestion>());
            Assert.That(question.Answer, Is.EqualTo("Framework"));
        }

        [Test]
        public void AddQuestion_ShouldAddQuestionToRepository()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Question",
                "Answer"
            );

            _questionRepo.AddQuestion(question);

            Assert.That(_questionRepo.GetAll().Count, Is.EqualTo(1));
        }

        [Test]
        public void GetQuestionById_ShouldReturnCorrectQuestion()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Question",
                "Answer"
            );

            _questionRepo.AddQuestion(question);

            Question? result = _questionRepo.GetQuestionById(question.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(question.Id));
        }

        [Test]
        public void GetQuestionById_ShouldReturnNull_WhenQuestionDoesNotExist()
        {
            Question? result = _questionRepo.GetQuestionById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RemoveQuestion_ShouldRemoveQuestionFromRepository()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Question",
                "Answer"
            );
            _questionRepo.AddQuestion(question);

            _questionRepo.RemoveQuestion(question);

            Assert.That(_questionRepo.GetAll().Count, Is.EqualTo(0));
        }

        [Test]
        public void UpdateQuestion_ShouldUpdateQuestionText()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Old text",
                "Answer"
            );
            _questionRepo.UpdateQuestion("TextOfQuestion", "New text", question);

            Assert.That(question.TextOfQuestion, Is.EqualTo("New text"));
        }

        [Test]
        public void UpdateQuestion_ShouldUpdateEnumProperty()
        {
            Question question = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Question",
                "Answer"
            );

            _questionRepo.UpdateQuestion(
                "Difficulty",
                DifficultyOfQuestion.Hard,
                question
            );

            Assert.That(question.Difficulty, Is.EqualTo(DifficultyOfQuestion.Hard));
        }

        [Test]
        public void GetAll_ShouldReturnAllQuestions()
        {
            Question q1 = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Easy,
                "Q1",
                "A1"
            );

            Question q2 = _questionRepo.CreateOpenQuestion(
                _subject,
                DifficultyOfQuestion.Normal,
                "Q2",
                "A2"
            );
            _questionRepo.AddQuestion(q1);
            _questionRepo.AddQuestion(q2);

            List<Question> result = _questionRepo.GetAll();

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}