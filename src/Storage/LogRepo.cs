using Сoursework.Models;

namespace Сoursework.Storage
{
    public class LogRepo
    {
        private List<Log> logs = new();
        private int _count = 1;

        public void SaveSessionResult(Test finishedTest, int sessionId)
        {
            foreach (Question question in finishedTest.Questions)
            {
                finishedTest.UserAnswers.TryGetValue(question.Id, out string? userAnswer);
                if (userAnswer == null)
                {
                    userAnswer = "No answer given.";
                }

                question.TotalAttempts++;
                if (question.CheckAnswer(userAnswer))
                {
                    question.CorrectAttempts++;
                }

                Log log = new Log { Id = _count++, SessionId = sessionId, QuestionId = question.Id, UserAnswer = userAnswer, WasCorrect = question.CheckAnswer(userAnswer)};

                logs.Add(log);
            }
        }

        public List<Log> GetWrongAnswersBySession(int sessionId)
        {
            return logs.Where(l => l.SessionId == sessionId && !l.WasCorrect).ToList();
        }

        public (int CorrectAnswers, int TotalQuestions) GetSessionScore(int sessionId)
        {
            List<Log> sessionLogs = logs.Where(l => l.SessionId == sessionId).ToList();
            int total = sessionLogs.Count;
            int correct = sessionLogs.Count(l => l.WasCorrect);

            return (correct, total);
        }

        public void PrintAllHistory()
        {
            Dictionary<int, List<Log>> groupedSessions = new();

            foreach (Log log in logs)
            {
                if (!groupedSessions.ContainsKey(log.SessionId))
                {
                    groupedSessions[log.SessionId] = new List<Log>();
                }

                groupedSessions[log.SessionId].Add(log);
            }

            foreach (KeyValuePair<int, List<Log>> session in groupedSessions)
            {
                int sessionId = session.Key;
                List<Log> sessionLogs = session.Value;

                int total = sessionLogs.Count;
                int correct = 0;

                foreach (Log log in sessionLogs)
                {
                    if (log.WasCorrect)
                    {
                        correct++;
                    }
                }

                double percentage = Math.Round((double)correct / total * 100, 1);

                Console.WriteLine($"Session id: {sessionId}, Accuracy: {percentage}%.");
            }
        }

        public void PrintWrongAnswersForSession(int sessionId)
        {
            List<Log> wrongLogs = GetWrongAnswersBySession(sessionId);

            if (wrongLogs.Count == 0)
            {
                Console.WriteLine($"Perfect score! No wrong answers found for session id {sessionId}.");
                return;
            }

            Console.WriteLine($"Wrong answers for session id {sessionId}");
            foreach (Log log in wrongLogs)
            {
                Console.WriteLine(log.ToString());
            }
        }

        public List<Log> GetAll() => logs;
    }
}