using Сoursework.Storage;

namespace Сoursework.Menus
{
    public class UserInterface
    {
        public static void UI()
        {
            if (!Directory.Exists(Serialization.DataDir))
            {
                Directory.CreateDirectory(Serialization.DataDir);
            }

            SubjectRepo subjectRepo = new();
            QuestionRepo questionRepo = new();
            LogRepo logRepo = new();

            Serialization.LoadAll(subjectRepo, questionRepo, logRepo);

            bool isTrue = true;
            while (isTrue)
            {
                Console.WriteLine("Hello! What would you like to do today?");
                Console.WriteLine("0 - exit\r\n1 - operate with subjects\r\n2 - operate with questions\r\n3 - start test\r\n4 - show history\r\n5 - show mistakes from session.");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        isTrue = false;
                        Serialization.SaveAll(subjectRepo, questionRepo, logRepo);
                        Console.WriteLine("Goodbye!");
                        break;
                    case "1":
                        SubjectMenu.InterfaceForSubject(subjectRepo);
                        break;
                    case "2":
                        QuestionMenu.InterfaceForQuestion(subjectRepo, questionRepo);
                        break;
                    case "3":
                        TestMenu.MenuForTest(subjectRepo, questionRepo, logRepo);
                        break;
                    case "4":
                        logRepo.PrintAllHistory();
                        break;
                    case "5":
                        UserInterface.SessionToInspect(logRepo);
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }

        private static void SessionToInspect(LogRepo logRepo)
        {
            Console.Write("Enter session id to inspect: ");
            try
            {
                int chosenSessionId = int.Parse(Console.ReadLine());

                logRepo.PrintWrongAnswersForSession(chosenSessionId);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
