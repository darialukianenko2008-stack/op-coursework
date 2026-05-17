using Сoursework.Models;

namespace Сoursework.Storage
{
    public class SubjectRepo
    {
        private List<Subject> _subjects = new();
        private int _count;

        public Subject CreateSubject(string name)
        {
            _count = _subjects.Count;
            return new Subject(_count, name);
        }

        public void AddSubject(Subject subject)
        {
            bool exists = _subjects.Any(s => s.Name.Trim().Equals(subject.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                throw new Exception("Subject with such name already exists.");
            }

            _subjects.Add(subject);
        }

        public void UpdateName(string newName, Subject subject)
        {
            subject.Name = newName;
        }

        public void RemoveSubject(Subject subject)
        {
            _subjects.Remove(subject);
        }

        public Subject? GetSubjectById(int id)
        {
            foreach (Subject subject in _subjects)
            {
                if(subject.Id == id)
                {
                    return subject;
                }
            }
            return null;
        }

        public void PrintSubjects()
        {
            foreach (Subject subject in _subjects)
            {
                Console.WriteLine(subject.ToString());
            }
        }

        public List<Subject> GetAll()
        {
            return _subjects;
        }
    }
}
