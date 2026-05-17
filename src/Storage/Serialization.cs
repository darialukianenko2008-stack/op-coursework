using System.Text.Json;
using System.Text.Json.Serialization;
using Сoursework.Models;

namespace Сoursework.Storage
{
    public class Serialization
    {
        private static readonly string _SubjectsFile = Path.Combine("Data", "subjects.json");
        private static readonly string _QuestionsFile = Path.Combine("Data", "questions.json");
        private static readonly string _LogsFile = Path.Combine("Data", "logs.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static void SaveSubjects(SubjectRepo subjectRepo)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_SubjectsFile);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(subjectRepo.GetAll(), Options);
                File.WriteAllText(_SubjectsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void LoadSubjects(SubjectRepo subjectRepo)
        {
            try
            {
                if (!File.Exists(_SubjectsFile)) return;

                string json = File.ReadAllText(_SubjectsFile);
                List<Subject>? list = JsonSerializer.Deserialize<List<Subject>>(json, Options);

                if (list != null)
                {
                    foreach (Subject subject in list)
                    {
                        subjectRepo.AddSubject(subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SaveQuestions(QuestionRepo questionRepo)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_QuestionsFile);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(questionRepo.GetAll(), Options);
                File.WriteAllText(_QuestionsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void LoadQuestions(QuestionRepo questionRepo)
        {
            try
            {
                if (!File.Exists(_QuestionsFile)) return;

                string json = File.ReadAllText(_QuestionsFile);
                List<Question>? list = JsonSerializer.Deserialize<List<Question>>(json, Options);

                if (list != null)
                {
                    foreach (Question question in list)
                    {
                        questionRepo.AddQuestion(question);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SaveLogs(LogRepo logRepo)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_LogsFile);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(logRepo.GetAll(), Options);
                File.WriteAllText(_LogsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void LoadLogs(LogRepo logRepo)
        {
            try
            {
                if (!File.Exists(_LogsFile)) return;

                string json = File.ReadAllText(_LogsFile);
                List<Log>? list = JsonSerializer.Deserialize<List<Log>>(json, Options);

                if (list != null)
                {
                    foreach (Log log in list)
                    {
                        logRepo.GetAll().Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SaveAll(SubjectRepo subjectRepo, QuestionRepo questionRepo, LogRepo logRepo)
        {
            SaveSubjects(subjectRepo);
            SaveQuestions(questionRepo);
            SaveLogs(logRepo);
            Console.WriteLine("All data saved successfully!");
        }

        public static void LoadAll(SubjectRepo subjectRepo, QuestionRepo questionRepo, LogRepo logRepo)
        {
            LoadSubjects(subjectRepo);
            LoadQuestions(questionRepo);

            foreach (Question question in questionRepo.GetAll())
            {
                if (question.Topic != null)
                {
                    Subject? originalSubject = subjectRepo.GetAll().FirstOrDefault(s => s.Id == question.Topic.Id);

                    if (originalSubject != null)
                    {
                        question.Topic = originalSubject;
                    }
                }
                else
                {
                    Subject? originalSubject = subjectRepo.GetAll().FirstOrDefault(s => s.Id == question.SubjectId);
                    if (originalSubject != null) question.Topic = originalSubject;
                }
            }

            LoadLogs(logRepo);
        }
    }
}
