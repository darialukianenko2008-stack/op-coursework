using System.Threading.Channels;
using Сoursework.Storage;

namespace Сoursework.Menus
{
    public class UserInterface
    {
        public static void UI()
        {
            SubjectRepo subjectRepo = new();
            QuestionRepo questionRepo = new();

            bool isTrue = true;
            while (isTrue)
            {
                Console.Clear();
                Console.WriteLine("Hello! What would you like to do today?");
                Console.WriteLine("0 - exit\r\n1 - operate with subjects\r\n2 - operate with questions\r\n3 - start test\r\n4 - show history\r\n");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        isTrue = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    case "1":
                        SubjectMenu.InterfaceForSubject(subjectRepo);
                        break;
                    case "2":
                        QuestionMenu.InterfaceForQuestion(subjectRepo, questionRepo);
                        break;
                    case "3":
                        TestMenu.MenuForTest();
                        break;
                    case "4":
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }

            }
        }
    }
}
