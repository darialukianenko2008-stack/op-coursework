using System.Reflection;
using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;

namespace Сoursework.Storage
{
    public class QuestionRepo
    {
        private List<Question> questions = new();
        private int _count = 0;

        public Question CreateSingleChoice(string topic, DifficultyOfQuestion diff, string text, string correctAnswer, List<string> options)
        {
            SingleAnswerOption question = new(_count++, topic, diff, text, correctAnswer, options);
            return question;
        }

        public Question CreateMultipleChoice(string topic, DifficultyOfQuestion diff, string text, string correctAnswers, List<string> options)
        {
            MultiAnswerOption question = new(_count++, topic, diff, text, correctAnswers, options);
            return question;
        }

        public Question CreateOpenQuestion(string topic, DifficultyOfQuestion diff, string text, string correctAnswer)
        {
            OpenQuestion question = new(_count++, topic, diff, text, correctAnswer);
            return question;
        }

        public void AddQuestion(Question question)
        {
            questions.Add(question);
        }

        public Question? GetQuestionById(int id)
        {
            return questions.Find(q => q.Id == id);
        }

        public void RemoveQuestion(Question question)
        {
            questions.Remove(question);
        }

        public void UpdateQuestion<T>(string propertyName, T newValue, Question question)
        {
            try
            {
                PropertyInfo prop = question.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (prop != null && prop.CanWrite)
                {
                    if (prop.PropertyType.IsEnum)
                    {
                        prop.SetValue(question, Enum.Parse(prop.PropertyType, newValue.ToString()));
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(newValue, prop.PropertyType);
                        prop.SetValue(question, convertedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PrintQuestions()
        {
            foreach (Question question in questions)
            {
                Console.WriteLine(question.ToString());
            }
        }

        public List<Question> GetAll() => questions;
    }
}