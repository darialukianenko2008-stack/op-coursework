using System.Reflection;
using Сoursework.Enums;
using Сoursework.Models;

namespace Сoursework.Storage
{
    public class QuestionRepo
    {
        private List<Question> questions = new();
        private int _count = -1;

        public Question CreateQuestion(string topic, DifficultyOfQuestion difficulty, string textOfQuestion, string answer)
        {
            _count++;
            return new Question(_count, topic, difficulty, textOfQuestion, answer);
        }

        public void AddQuestion(Question question)
        {
            questions.Add(question);
        }

        public Question? GetQuestionById(int id)
        {
            foreach (Question question in questions)
            {
                if (question.Id == id)
                {
                    return question;
                }
            }
            return null;
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
                    if (prop.PropertyType.IsAssignableFrom(typeof(T)))
                    {
                        prop.SetValue(question, newValue);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(newValue, prop.PropertyType);
                        prop.SetValue(question, convertedValue);
                    }
                }
            }
            catch(NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
        }

        public void PrintQuestions()
        {
            foreach (Question question in questions)
            {
                Console.WriteLine(question.ToString());
            }
        }
    }
}
