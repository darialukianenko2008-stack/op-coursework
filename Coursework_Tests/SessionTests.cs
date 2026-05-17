using NUnit.Framework.Legacy;
using System.Reflection;
using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;
using Сoursework.Storage;

namespace Coursework_Tests
{
    public class SessionTests
    {
        private Subject _subject;
        private QuestionRepo _questionRepo;

        [SetUp]
        public void Setup()
        {
            _subject = new Subject(1, "Programming");
            _questionRepo = new QuestionRepo();
        }

        [Test]
        public void Constructor_ShouldInitializeFields()
        {
            List<Question> questions = new();

            Test test = new Test(1, questions);

            Assert.That(test.Id, Is.EqualTo(1));
            Assert.That(test.Questions, Is.EqualTo(questions));
            Assert.That(test.UserAnswers.Count, Is.EqualTo(0));
        }

        [Test]
        public void AnswerQuestion_ShouldSaveUserAnswer()
        {
            Test test = new Test(1, new List<Question>());

            test.AnswerQuestion(5, "C#");

            Assert.That(test.UserAnswers.ContainsKey(5), Is.True);
            Assert.That(test.UserAnswers[5], Is.EqualTo("C#"));
        }

        [Test]
        public void CreateTest_ShouldCreateFilteredTest()
        {
            Question q1 = new OpenQuestion(
                1,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q1",
                "A1"
            );

            Question q2 = new OpenQuestion(
                2,
                _subject,
                DifficultyOfQuestion.Hard,
                "Q2",
                "A2"
            );

            _questionRepo.AddQuestion(q1);
            _questionRepo.AddQuestion(q2);

            Test test = new Test(1, new List<Question>());

            Test created = test.CreateTest(
                1,
                _subject,
                q => q.Difficulty == DifficultyOfQuestion.Easy,
                _questionRepo
            );

            Assert.That(created.Questions.Count, Is.EqualTo(1));
            Assert.That(created.Questions[0].Difficulty, Is.EqualTo(DifficultyOfQuestion.Easy));
        }

        [Test]
        public void CreateTest_ShouldThrowException_WhenNoQuestionsExist()
        {
            Test test = new Test(1, new List<Question>());

            Assert.Throws<Exception>(() =>
            {
                test.CreateTest(
                    1,
                    _subject,
                    q => true,
                    _questionRepo
                );
            });
        }

        [Test]
        public void CreateTest_ShouldThrowException_WhenNumberOfQuestionLessThanZero()
        {
            Question q1 = new OpenQuestion(
                1,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q1",
                "A1"
            );

            Question q2 = new OpenQuestion(
                2,
                _subject,
                DifficultyOfQuestion.Hard,
                "Q2",
                "A2"
            );

            _questionRepo.AddQuestion(q1);
            _questionRepo.AddQuestion(q2);
            Test test = new Test(1, new List<Question>());

            Assert.Throws<Exception>(() =>
            {
                test.CreateTest(
                    -2,
                    _subject,
                    q => true,
                    _questionRepo
                );
            });
        }

        [Test]
        public void ShuffleTest_ShouldKeepSameNumberOfQuestions()
        {
            Question q1 = new OpenQuestion(
                1,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q1",
                "A1"
            );

            Question q2 = new OpenQuestion(
                2,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q2",
                "A2"
            );

            Test test = new Test(1, new List<Question> { q1, q2 });

            test.ShuffleTest();

            Assert.That(test.Questions.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShuffleTest_ShouldNotRemoveQuestions()
        {
            Question q1 = new OpenQuestion(
                1,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q1",
                "A1"
            );

            Question q2 = new OpenQuestion(
                2,
                _subject,
                DifficultyOfQuestion.Easy,
                "Q2",
                "A2"
            );

            List<Question> original = new() { q1, q2 };

            Test test = new Test(1, original);

            test.ShuffleTest();
            Assert.That(test.Questions.Select(q => q.Id), Is.EquivalentTo(original.Select(q => q.Id)));
        }

        [Test]
        public void ShuffleTest_ShouldShuffleOptions_ForChoiceQuestions()
        {
            List<string> options = new()
            {
                "A",
                "B",
                "C",
                "D"
            };

            Question question = new SingleAnswerOption(
                1,
                _subject,
                DifficultyOfQuestion.Easy,
                "Question",
                "A",
                options
            );

            Test test = new Test(1, new List<Question> { question });

            test.ShuffleTest();

            PropertyInfo? prop = question.GetType().GetProperty("Options");

            List<string>? shuffled = prop?.GetValue(question) as List<string>;

            Assert.That(shuffled, Is.Not.Null);
            Assert.That(shuffled, Is.EquivalentTo(options));
        }
    }
}
