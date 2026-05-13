using Сoursework.Enums;

namespace Сoursework.Models.TypesOfQuestion
{
    public class MultiAnswerOption : Question
    {
        public List<string> Options { get; set; }


        public MultiAnswerOption(int id, string topic, DifficultyOfQuestion diff, string text, string answer, List<string> options) : base(id, topic, diff, text, answer)
        {
            Options = options;
        }

        public override bool CheckAnswer(string userAnswer)
        {
            if (!string.IsNullOrWhiteSpace(userAnswer))
            {
                List<int> userList = ParseStringToList(userAnswer);
                List<int> correctList = ParseStringToList(Answer);

                if (userList.Count == 0 || userList.Count != correctList.Count)
                {
                    return false;
                }

                userList.Sort();
                correctList.Sort();

                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i] != correctList[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        private List<int> ParseStringToList(string input)
        {
            List<int> result = new List<int>();
            string[] parts = input.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int val))
                {
                    result.Add(val);
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
