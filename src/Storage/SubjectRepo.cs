using Сoursework.Models;

namespace Сoursework.Storage
{
    public class SubjectRepo
    {
        private List<Subject> _subjects = new();
        private int _count = -1;

        public Subject CreateSubject(string name)
        {
            _count++;
            return new Subject(_count, name);
        }

        public void AddSubject(Subject subject)
        {
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
    }
}
