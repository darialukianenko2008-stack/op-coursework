using Сoursework.Interfaces;

namespace Сoursework.Models
{
    public record Log : IIdentifiable, IPrintable
    {
        public int Id { get; }
        public int SessionId { get; }
        public bool WasCorrect { get; }
    }
}
