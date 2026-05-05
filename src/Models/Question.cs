using Сoursework.Enums;
using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public class Question : IIdentifiable, IPrintable
    {
        public int Id { get; }
        public string Topic { get; set; }
        public DifficultyOfQuestion Difficulty { get; set; }
        public string TextOfQuestion { get; set; }
        public string Answer { get; set; }
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
