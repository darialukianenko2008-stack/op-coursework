using System.Text.Json;
using System.Text.Json.Serialization;
using Сoursework.Models;

namespace Сoursework.Storage
{
    public class Serialization
    {
        public static readonly string DataDir = Path.Combine(AppContext.BaseDirectory, "Data");

        private static readonly string _subjects = Path.Combine(DataDir, "subjects.json");

        private static readonly string _questions = Path.Combine(DataDir, "questions.json");

        private static readonly string _logs = Path.Combine(DataDir, "logs.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static void SaveSubjects(SubjectRepo subjectRepo)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_subjects);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(subjectRepo.GetAll(), Options);
                File.WriteAllText(_subjects, json);
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
                if (!File.Exists(_subjects)) return;

                string json = File.ReadAllText(_subjects);
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
                string? directory = Path.GetDirectoryName(_questions);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(questionRepo.GetAll(), Options);
                File.WriteAllText(_questions, json);
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
                if (!File.Exists(_questions)) return;

                string json = File.ReadAllText(_questions);
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
                string? directory = Path.GetDirectoryName(_logs);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(logRepo.GetAll(), Options);
                File.WriteAllText(_logs, json);
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
                if (!File.Exists(_logs)) return;

                string json = File.ReadAllText(_logs);
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
