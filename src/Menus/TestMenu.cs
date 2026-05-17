using System.Reflection;
using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Storage;

namespace Сoursework.Menus
{
    public class TestMenu
    {
        private readonly QuestionRepo _questionRepo;
        private readonly SubjectRepo _subjectRepo;
        private readonly LogRepo _logRepo;

        private static int _testIdCounter = 1;

        public TestMenu(SubjectRepo subjectRepo, QuestionRepo questionRepo, LogRepo logRepo)
        {
            _subjectRepo = subjectRepo;
            _questionRepo = questionRepo;
            _logRepo = logRepo;
        }

        public static void MenuForTest(SubjectRepo subjectRepo, QuestionRepo questionRepo, LogRepo logRepo)
        {
            TestMenu menu = new(subjectRepo, questionRepo, logRepo);

            bool isTrue = true;
            while (isTrue)
            {
                Console.WriteLine();
                Console.WriteLine("0 - return\r\n1 - generate and start new test");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        isTrue = false;
                        break;
                    case "1":
                        menu.ExecuteQuizSession();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        private void ExecuteQuizSession()
        {
            Console.Clear();
            try
            {
                Test activeTest = ConfigureAndGenerateTest();

                RunQuizLoop(activeTest);

                DisplayQuizResults(activeTest);
                Serialization.SaveLogs(_logRepo);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Test ConfigureAndGenerateTest()
        {
            _subjectRepo.PrintSubjects();
            Console.Write("Choose subject ID for the test: ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                throw new FormatException("Subject ID must be an integer.");
            }

            Subject? subject = _subjectRepo.GetSubjectById(subjectId);
            if (subject == null)
            {
                throw new InvalidOperationException("Subject not found.");
            }

            Console.WriteLine("Choose difficulty level:");
            Console.WriteLine("1 - Easy\n2 - Normal\n3 - Hard\n4 - Any difficulty (All questions)");
            string diffInput = Console.ReadLine();

            Predicate<Question> difficultyPredicate = q => true;

            if (diffInput == "1")
            {
                difficultyPredicate = q => q.Difficulty == DifficultyOfQuestion.Easy;
            }
            else if (diffInput == "2")
            {
                difficultyPredicate = q => q.Difficulty == DifficultyOfQuestion.Normal;
            }
            else if (diffInput == "3")
            {
                difficultyPredicate = q => q.Difficulty == DifficultyOfQuestion.Hard;
            }
            else if (diffInput != "4")
            {
                throw new ArgumentException("Invalid difficulty choice.");
            }

            Console.Write("How many questions do you want in this test? ");
            if (!int.TryParse(Console.ReadLine(), out int qCount) || qCount <= 0)
            {
                throw new ArgumentException("Count must be greater than 0.");
            }

            Test generator = new Test(_testIdCounter++, new List<Question>());
            generator.Questions = _questionRepo.GetAll();

            Test activeTest = generator.CreateTest(qCount, subject, difficultyPredicate, _questionRepo);

            if (activeTest == null || activeTest.Questions.Count == 0)
            {
                throw new InvalidOperationException("Not enough questions for this subject/difficulty in the database.");
            }

            activeTest.ShuffleTest();
            return activeTest;
        }

        private void RunQuizLoop(Test activeTest)
        {
            Console.Clear();
            Console.WriteLine($"Good luck! The test starts now.");
            Console.WriteLine($"Questions: {activeTest.Questions.Count}");

            int currentQuestionNumber = 1;
            foreach (Question question in activeTest.Questions)
            {
                Console.WriteLine($"Question {currentQuestionNumber}/{activeTest.Questions.Count}:");
                Console.WriteLine(question.TextOfQuestion);

                PropertyInfo? optionsProp = question.GetType().GetProperty("Options");
                if (optionsProp != null && optionsProp.GetValue(question) is List<string> options)
                {
                    for (int i = 0; i < options.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {options[i]}");
                    }
                }

                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                activeTest.AnswerQuestion(question.Id, userAnswer);
                Console.WriteLine();
                currentQuestionNumber++;
            }
        }

        private void DisplayQuizResults(Test activeTest)
        {
            Console.Clear();
            Console.WriteLine("You finished the test. Congrats!");
            _logRepo.SaveSessionResult(activeTest, activeTest.Id);

            (int Correct, int All) score = _logRepo.GetSessionScore(activeTest.Id);

            Console.WriteLine($"Your result: {score.Correct} out of {score.All} correct.");
        }
    }
}
