using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Storage;

namespace Сoursework.Menus
{
    public class QuestionMenu
    {
        private readonly QuestionRepo _questionRepo;
        private readonly SubjectRepo _subjectRepo;

        public QuestionMenu(SubjectRepo subjectRepo, QuestionRepo questionRepo)
        {
            _subjectRepo = subjectRepo;
            _questionRepo = questionRepo;
        }

        public static void InterfaceForQuestion(SubjectRepo subjectRepo, QuestionRepo questionRepo)
        {
            QuestionMenu menu = new(subjectRepo, questionRepo);

            bool isTrue = true;
            while (isTrue)
            {
                Console.WriteLine();
                Console.WriteLine("0 - return\r\n1 - add single choice question\r\n2 - add multiple choice question\r\n3 - add open question\r\n4 - remove question\r\n5 - update question\r\n6 - show available questions.");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        isTrue = false;
                        break;
                    case "1":
                        menu.ExecuteAddSingleChoice();
                        break;
                    case "2":
                        menu.ExecuteAddMultipleChoice();
                        break;
                    case "3":
                        menu.ExecuteAddOpenQuestion();
                        break;
                    case "4":
                        menu.ExecuteRemoveQuestion();
                        break;
                    case "5":
                        menu.ExecuteUpdateQuestion();
                        break;
                    case "6":
                        Console.Clear();
                        menu._questionRepo.PrintQuestions();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        private void ExecuteAddSingleChoice()
        {
            Console.Clear();
            try
            {
                Subject subject = ChooseSubject();
                DifficultyOfQuestion diff = ChooseDifficulty();

                Console.Write("Input question text: ");
                string text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Question text can't be empty.");
                }

                List<string> options = InputOptions();

                Console.Write("Input correct answer: ");
                string correctAnswer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(correctAnswer))
                {
                    throw new ArgumentException("Correct answer can't be empty.");
                }

                Question q = _questionRepo.CreateSingleChoice(subject, diff, text, correctAnswer, options);
                _questionRepo.AddQuestion(q);
                Console.WriteLine("Single choice question added successfully!");
                Serialization.SaveQuestions(_questionRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteAddMultipleChoice()
        {
            Console.Clear();
            try
            {
                Subject subject = ChooseSubject();
                DifficultyOfQuestion diff = ChooseDifficulty();

                Console.Write("Input question text: ");
                string text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Question text can't be empty.");
                }

                List<string> options = InputOptions();

                Console.Write("Input correct answers: ");
                string correctAnswers = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(correctAnswers))
                {
                    throw new ArgumentException("Correct answers can't be empty.");
                }

                Question q = _questionRepo.CreateMultipleChoice(subject, diff, text, correctAnswers, options);
                _questionRepo.AddQuestion(q);
                Console.WriteLine("Multiple choice question added successfully!");
                Serialization.SaveQuestions(_questionRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteAddOpenQuestion()
        {
            Console.Clear();
            try
            {
                Subject subject = ChooseSubject();
                DifficultyOfQuestion diff = ChooseDifficulty();

                Console.Write("Input question text: ");
                string text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Question text can't be empty.");

                Console.Write("Input correct answer: ");
                string correctAnswer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(correctAnswer))
                {
                    throw new ArgumentException("Correct answer can't be empty.");
                }

                Question q = _questionRepo.CreateOpenQuestion(subject, diff, text, correctAnswer);
                _questionRepo.AddQuestion(q);
                Console.WriteLine("Open question added successfully!");
                Serialization.SaveQuestions(_questionRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteRemoveQuestion()
        {
            Console.Clear();
            _questionRepo.PrintQuestions();
            try
            {
                Console.Write("Input question id to remove: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new FormatException("Id must be an integer.");
                }

                Question question = _questionRepo.GetQuestionById(id);
                if (question == null)
                {
                    throw new InvalidOperationException("Question with such id doesn't exist.");
                }

                _questionRepo.RemoveQuestion(question);
                Console.WriteLine("Question deleted successfully.");
                Serialization.SaveQuestions(_questionRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteUpdateQuestion()
        {
            Console.Clear();
            _questionRepo.PrintQuestions();
            try
            {
                Console.Write("Input question id to update: ");
                if (!int.TryParse(Console.ReadLine(), out int id)) throw new FormatException("Id must be an integer.");

                Question question = _questionRepo.GetQuestionById(id);
                if (question == null)
                {
                    throw new InvalidOperationException("Question with such id doesn't exist.");
                }

                Console.Write("Input property name to update (Difficulty, TextOfQuestion, Answer, Options): ");
                string propName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(propName))
                {
                    throw new ArgumentException("Property name can't be empty.");
                }

                Console.Write("Input new value: ");
                string newValue = Console.ReadLine();

                _questionRepo.UpdateQuestion(propName, newValue, question);
                Console.WriteLine("Question updated successfully!");
                Serialization.SaveQuestions(_questionRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Subject ChooseSubject()
        {
            _subjectRepo.PrintSubjects();
            Console.Write("Input subject Id for this question: ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                throw new FormatException("Subject Id must be an integer.");
            }

            Subject? subject = _subjectRepo.GetSubjectById(subjectId);
            if (subject == null)
            {
                throw new InvalidOperationException("Subject with such Id doesn't exist.");
            }

            return subject;
        }

        private DifficultyOfQuestion ChooseDifficulty()
        {
            Console.Write("Input difficulty (Easy = 0, Medium = 1, Hard = 2): ");
            if (!int.TryParse(Console.ReadLine(), out int diffValue) || diffValue < 0 || diffValue > 2)
            {
                throw new ArgumentException("Invalid difficulty choice.");
            }
            return (DifficultyOfQuestion)diffValue;
        }

        private List<string> InputOptions()
        {
            List<string> options = new();
            Console.Write("How many answer options? ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count < 2)
            {
                throw new ArgumentException("Must be at least 2 options.");
            }

            for (int i = 0; i < count; i++)
            {
                Console.Write($"Input option {i + 1}: ");
                string opt = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(opt))
                {
                    throw new ArgumentException("Option can't be empty.");
                }
                options.Add(opt);
            }
            return options;
        }
    }
}