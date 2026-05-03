using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public class Subject : IIdentifiable, IPrintable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Subject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}.";
        }
    }
}
