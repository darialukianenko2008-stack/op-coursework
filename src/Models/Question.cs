using Сoursework.Enums;
using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public abstract class Question : IIdentifiable, IPrintable
    {
        public int Id { get; }
        public Subject Topic { get; }
        public DifficultyOfQuestion Difficulty { get; set; }
        public string TextOfQuestion { get; set; }
        public string Answer { get; set; }
        public int TotalAttempts { get; private set; } = 0;
        public int CorrectAttempts { get; private set; } = 0;

        public double SuccessRate
        {
            get
            {
                if (TotalAttempts == 0) return 0;
                return (double)CorrectAttempts / TotalAttempts * 100;
            }
        }

        public Question(int id, Subject topic, DifficultyOfQuestion difficulty, string textOfQuestion, string answer)
        {
            Id = id;
            Topic = topic;
            Difficulty = difficulty;
            TextOfQuestion = textOfQuestion;
            Answer = answer;
        }

        public abstract bool CheckAnswer(string answer);
        public abstract string PrintQuestion();

        public void UpdateStatistics(bool isCorrect)
        {
            TotalAttempts++;
            if (isCorrect)
            {
                CorrectAttempts++;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, {Topic}, {Difficulty}, {TextOfQuestion}, Answer: {Answer}, Success rate: {SuccessRate}.";
        }
    }
}
