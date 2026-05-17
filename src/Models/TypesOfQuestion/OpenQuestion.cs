using Сoursework.Enums;

namespace Сoursework.Models.TypesOfQuestion
{
    public class OpenQuestion : Question
    {
        public string CorrectAnswer { get; set; }

        public OpenQuestion(int id, Subject topic, DifficultyOfQuestion diff, string text, string correctAnswer) : base(id, topic, diff, text, correctAnswer)
        {
            CorrectAnswer = correctAnswer;
        }

        public OpenQuestion() { }

        public override bool CheckAnswer(string userAnswer) =>
            userAnswer.Trim().Equals(Answer, StringComparison.OrdinalIgnoreCase);

        public override string PrintQuestion() => TextOfQuestion;
    }
}
