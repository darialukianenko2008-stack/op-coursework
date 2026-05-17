using System.Reflection;
using Сoursework.Enums;
using Сoursework.Models;
using Сoursework.Models.TypesOfQuestion;

namespace Сoursework.Storage
{
    public class QuestionRepo
    {
        private List<Question> _questions = new();
        private int _count = 0;

        public Question CreateSingleChoice(Subject topic, DifficultyOfQuestion diff, string text, string correctAnswer, List<string> options)
        {
            SingleAnswerOption question = new(_count++, topic, diff, text, correctAnswer, options);
            return question;
        }

        public Question CreateMultipleChoice(Subject topic, DifficultyOfQuestion diff, string text, string correctAnswers, List<string> options)
        {
            MultiAnswerOption question = new(_count++, topic, diff, text, correctAnswers, options);
            return question;
        }

        public Question CreateOpenQuestion(Subject topic, DifficultyOfQuestion diff, string text, string correctAnswer)
        {
            OpenQuestion question = new(_count++, topic, diff, text, correctAnswer);
            return question;
        }

        public void AddQuestion(Question question)
        {
            _questions.Add(question);
        }

        public Question? GetQuestionById(int id)
        {
            return _questions.Find(q => q.Id == id);
        }

        public void RemoveQuestion(Question question)
        {
            _questions.Remove(question);
        }

        public void UpdateQuestion<T>(string propertyName, T newValue, Question question)
        {
            try
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
                PropertyInfo? prop = question.GetType().GetProperty(propertyName, flags);

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
            foreach (Question question in _questions)
            {
                Console.WriteLine(question.ToString());
            }
        }

        public List<Question> GetAll() => _questions;
    }
}