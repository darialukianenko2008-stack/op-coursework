using System.Text.Json.Serialization;
using Сoursework.Enums;
using Сoursework.Interfaces;
using Сoursework.Models.TypesOfQuestion;

namespace Сoursework.Models
{
    [JsonDerivedType(typeof(SingleAnswerOption), typeDiscriminator: "single")]
    [JsonDerivedType(typeof(MultiAnswerOption), typeDiscriminator: "multiple")]
    [JsonDerivedType(typeof(OpenQuestion), typeDiscriminator: "open")]
    public abstract class Question : IIdentifiable, IPrintable
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Subject Topic { get; set; }
        public int SubjectId { get; set; }
        public DifficultyOfQuestion Difficulty { get; set; }
        public string TextOfQuestion { get; set; }
        public string Answer { get; set; }
        public int TotalAttempts { get; set; }
        public int CorrectAttempts { get; set; }

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
            SubjectId = topic.Id;
        }

        public Question() { }

        public abstract bool CheckAnswer(string answer);
        public abstract string PrintQuestion();

        public override string ToString()
        {
            return $"Id: {Id}, {Topic.Name}, {Difficulty}, {TextOfQuestion}, Answer: {Answer}, Success rate: {SuccessRate}.";
        }
    }
}
