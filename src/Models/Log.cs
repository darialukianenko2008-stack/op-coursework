using System.Net.NetworkInformation;
using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public record Log : IIdentifiable, IPrintable
    {
        public int Id { get; init; }
        public int SessionId { get; init; }
        public int QuestionId { get; init; }
        public string UserAnswer { get; init; } = string.Empty;
        public bool WasCorrect { get; init; }

        public override string ToString()
        {
            string status = "No";
            if (WasCorrect)
            {
                status = "Yes";
            }
            return $"Id: {Id}, Session: {SessionId}, Question Id: {QuestionId}, Your answer: {UserAnswer}, Was correct? : {status}.";
        }
    }
}
