using Сoursework.Models;
using Сoursework.Storage;

namespace Сoursework.Menus
{
    public class SubjectMenu
    {
        private readonly SubjectRepo _repo;

        public SubjectMenu(SubjectRepo repo)
        {
            _repo = repo;
        }

        public static void InterfaceForSubject(SubjectRepo repo)
        {
            SubjectMenu menu = new(repo);

            bool isTrue = true;
            while (isTrue)
            {
                Console.WriteLine();
                Console.WriteLine("0 - return\r\n1 - add subject\r\n2 - update subject's name\r\n3 - remove subject\r\n4 - show available subjects.");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        isTrue = false;
                        break;
                    case "1":
                        menu.ExecuteAddSubject();
                        break;
                    case "2":
                        menu.ExecuteUpdateSubject();
                        break;
                    case "3":
                        menu.ExecuteRemoveSubject();
                        break;
                    case "4":
                        menu._repo.PrintSubjects();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        private void ExecuteAddSubject()
        {
            Console.Clear();
            try
            {
                Console.Write("Input name: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name of the subject can't be empty.");
                }

                Subject newSubject = _repo.CreateSubject(name);
                _repo.AddSubject(newSubject);
                Console.WriteLine("Added successfully!");
                Serialization.SaveSubjects(_repo);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteUpdateSubject()
        {
            _repo.PrintSubjects();

            try
            {
                Console.Write("Input id: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new FormatException("Id must be an intenger.");
                }

                Subject subject = _repo.GetSubjectById(id);

                if (subject == null)
                {
                    throw new InvalidOperationException($"Subject with such id doesn't exist.");
                }

                Console.Write("Enter new name for subject: ");
                string newName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new ArgumentException("Name of the subject can't be empty.");
                }

                _repo.UpdateName(newName, subject);
                Console.WriteLine("Name of the subject updated successfully!");
                Serialization.SaveSubjects(_repo);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecuteRemoveSubject()
        {
            Console.Clear();
            _repo.PrintSubjects();

            try
            {
                Console.Write("Input id:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    throw new FormatException("Id must be an intenger.");
                }

                Subject? subject = _repo.GetSubjectById(id);

                if (subject == null)
                {
                    throw new InvalidOperationException($"Subject with such id doesn't exist.");
                }

                _repo.RemoveSubject(subject);
                Console.WriteLine("Deleted successfully.");
                Serialization.SaveSubjects(_repo);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}