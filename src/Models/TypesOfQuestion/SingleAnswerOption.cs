using Сoursework.Enums;

namespace Сoursework.Models.TypesOfQuestion
{
    public class SingleAnswerOption : Question
    {
        public List<string> Options { get; set; } 

        public SingleAnswerOption(int id, string topic, DifficultyOfQuestion diff, string text, string answer, List<string> options) : base(id, topic, diff, text, answer)
        {
            Options = options;
        }

        public override bool CheckAnswer(string userAnswer)
        {
            if (int.TryParse(userAnswer, out int userIndex))
            {
                return Options[userIndex - 1] == Answer;
            }
            return false;
        }

        public override string PrintQuestion()
        {
            string optionsOfAnswer = string.Empty;

            for (int i = 0; i < Options.Count; i++)
            {
                optionsOfAnswer += $"{i + 1}. " + Options[i] + "  ";
            }

            return $"{TextOfQuestion}\r\n{optionsOfAnswer}";
        }
    }
}
