using Сoursework.Models;

namespace Сoursework.Storage
{
    public class SubjectRepo
    {
        private List<Subject> subjects = new();
        private int count = -1;

        public Subject CreateSubject(string name)
        {
            count++;
            return new Subject(count, name);
        }

        public void AddSubject(Subject subject)
        {
            subjects.Add(subject);
        }

        public void UpdateName(string newName, Subject subject)
        {
            subject.Name = newName;
        }

        public void RemoveSubject(Subject subject)
        {
            subjects.Remove(subject);
        }

        public Subject? GetSubjectById(int id)
        {
            foreach (Subject subject in subjects)
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
            foreach (Subject subject in subjects)
            {
                Console.WriteLine(subject.ToString());
            }
        }
    }
}
