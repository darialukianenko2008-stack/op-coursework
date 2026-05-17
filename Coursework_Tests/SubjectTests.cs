using Сoursework.Models;
using Сoursework.Storage;

namespace Coursework_Tests
{
    public class SubjectTests
    {
        SubjectRepo repo;

        [SetUp]
        public void Setup()
        {
            repo = new();
        }

        [Test]
        public void CreateSubject_Positive()
        {
            string expectedName = "Math";

            Subject result = repo.CreateSubject(expectedName);
            int expectedId = 0;

            Assert.That(result, Is.Not.Null);
            Assert.That(expectedName, Is.EqualTo(result.Name));
            Assert.That(expectedId, Is.EqualTo(result.Id));
        }

        [Test]
        public void AddSubject_Positive()
        {
            Subject subject = new Subject(1, "History");

            repo.AddSubject(subject);
            List<Subject> allSubjects = repo.GetAll();
            int expectedNumber = 1;

            Assert.That(expectedNumber, Is.EqualTo(allSubjects.Count));
            Assert.That(allSubjects.Contains(subject), Is.True);
        }

        [Test]
        public void AddSubject_Negative_SubjectAlreadyExists()
        {
            Subject subject1 = new Subject(1, "History");
            Subject subject2 = new Subject(2, "history");

            repo.AddSubject(subject1);
            string expectedMessage = "Subject with such name already exists.";
            Exception ex = Assert.Throws<Exception>(() => repo.AddSubject(subject2));
            Assert.That(expectedMessage, Is.EqualTo(ex.Message));
        }

        [Test]
        public void UpdateName_Positive()
        {
            Subject subject = new(1, "Old Name");
            string newName = "New Name";

            repo.UpdateName(newName, subject);

            Assert.That(newName, Is.EqualTo(subject.Name));
        }

        [Test]
        public void RemoveSubject_Positive()
        {
            Subject subject = new Subject(1, "History");
            repo.AddSubject(subject);

            repo.RemoveSubject(subject);
            List<Subject> subjects = repo.GetAll();

            Assert.That(subjects.Contains(subject), Is.False);
        }

        [Test]
        public void GetSubjectById_Positive()
        {
            Subject subject1 = new(10, "Physics");
            Subject subject2 = new(20, "Chemistry");
            repo.AddSubject(subject1);
            repo.AddSubject(subject2);

            Subject? result = repo.GetSubjectById(20);
            int expectedId = 20;

            Assert.That(result, Is.Not.Null);
            Assert.That(subject2, Is.EqualTo(result));
            Assert.That(expectedId, Is.EqualTo(result.Id));
        }

        [Test]
        public void GetSubjectById_Negative_IdDoesNotExist_ShouldReturnNull()
        {
            Subject subject = new(1, "Biology");
            repo.AddSubject(subject);

            Subject? result = repo.GetSubjectById(2);

            Assert.That(result, Is.Null);
        }
    }
}
