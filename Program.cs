using Сoursework.Models;
using Сoursework.Storage;

class Program
{
    public static void Main(string[] args)
    {
        SubjectRepo subjectRepo = new SubjectRepo();
        subjectRepo.AddSubject(subjectRepo.CreateSubject("Math"));
        subjectRepo.AddSubject(subjectRepo.CreateSubject("Op"));
        subjectRepo.PrintSubjects();
        Subject subject = subjectRepo.GetSubjectById(1);
        subjectRepo.UpdateName("PE", subject);
        subjectRepo.PrintSubjects();
        subjectRepo.RemoveSubject(subject);
        subjectRepo.PrintSubjects();
    }
}