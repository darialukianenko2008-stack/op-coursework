using Сoursework.Enums;
using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public class Question : IIdentifiable, IPrintable
    {
        public int Id { get; }
        public string Topic { get; }
        public DifficultyOfQuestion Difficulty { get; }
        public string TextOfQuestion { get; }
        public string Answer { get; }
        public double SuccessRate { get; set; } = 0;

        public Question(int id, string topic, DifficultyOfQuestion difficulty, string textOfQuestion, string answer)
        {
            Id = id;
            Topic = topic;
            Difficulty = difficulty;
            TextOfQuestion = textOfQuestion;
            Answer = answer;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Topic: {Topic}, Difficulty: {Difficulty}, Text of question: {TextOfQuestion}, Answer: {Answer}";
        }
    }
}
