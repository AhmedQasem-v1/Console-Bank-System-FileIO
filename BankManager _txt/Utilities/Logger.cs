namespace BankProject
{
    public static class Logger
    {
        private static string logFilePath = "transaction_log.txt";
        public static void LogTransaction(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now}]{message}";
                using (StreamWriter swl = new StreamWriter(logFilePath, append: true))
                {
                    swl.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($" Warning: Log failed. Reason: {ex.Message}");
                Console.WriteLine($"Data that we failed to save: {message}");
            }
        }





    }
}