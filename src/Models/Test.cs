using System.Reflection;
using Сoursework.Storage;

namespace Сoursework.Models
{
    public class Test
    {
        public int Id { get; set; } = -1;
        public List<Question> Questions { get; set; } = new();
        public Dictionary<int, string> UserAnswers { get; set; } = new();

        public Test(int id, List<Question> questions)
        {
            Id = id;
            Questions = questions;
        }

        public void AnswerQuestion(int questionId, string answer)
        {
            UserAnswers[questionId] = answer;
        }

        public Test CreateTest(int numberOfQuestions, Subject subject, Predicate<Question> predicate, QuestionRepo questionRepo)
        {
            Questions = questionRepo.GetAll();
            if (Questions.Count == 0)
            {
                throw new Exception("Create some questions first.");
            }
            if (numberOfQuestions <= 0)
            {
                throw new Exception("Test has to contain at least 1 question.");
            }

            List<Question> sorted = Questions.Where(q => q.Topic == subject).Where(q => predicate(q)).ToList();

            Random random = new();
            List<Question> filtered = sorted.OrderBy(q => random.Next()).Take(numberOfQuestions).ToList();

            return new Test(Id++, filtered);
        }

        public void ShuffleTest()
        {
            try
            {
                Random random = new();

                Questions = Questions.OrderBy(q => random.Next()).ToList();

                foreach (Question question in Questions)
                {
                    PropertyInfo? optionsProp = question.GetType().GetProperty("Options", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (optionsProp != null && optionsProp.GetValue(question) is List<string> options)
                    {
                        List<string> shuffledOptions = options.OrderBy(o => random.Next()).ToList();
                        optionsProp.SetValue(question, shuffledOptions);
                    }
                }
            }
            catch (TargetException tex)
            {
                Console.WriteLine(tex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
