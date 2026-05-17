using Сoursework.Enums;

namespace Сoursework.Models.TypesOfQuestion
{
    public class MultiAnswerOption : Question
    {
        public List<string> Options { get; set; }

        public MultiAnswerOption(int id, Subject topic, DifficultyOfQuestion diff, string text, string answer, List<string> options) : base(id, topic, diff, text, answer)
        {
            Options = options;
        }
        
        public MultiAnswerOption() { }

        public override bool CheckAnswer(string userAnswer)
        {
            if (!string.IsNullOrWhiteSpace(userAnswer))
            {
                List<string> userList = ParseStringToList(userAnswer);
                List<string> correctList = ParseStringToList(Answer);

                if (userList.Count == 0 || userList.Count != correctList.Count)
                {
                    return false;
                }

                userList.Sort(StringComparer.OrdinalIgnoreCase);
                correctList.Sort(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < userList.Count; i++)
                {
                    if (!userList[i].Equals(correctList[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        private List<string> ParseStringToList(string input)
        {
            List<string> result = new();
            string[] parts = input.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    result.Add(trimmed);
                }
            }
            return result;
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
