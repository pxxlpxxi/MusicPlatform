namespace MusicPlatform.Logging
{
    internal static class DatabaseLogger
    {
        private const string LogFile = "database.log"; // MusicPlatform\bin\Debug\net10.0\database.log

        internal static void Log(string operation, string entity, string details)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logEntry = $"{timestamp} | {operation} | {entity} | {details}{Environment.NewLine}";

            File.AppendAllText(LogFile, logEntry);
        }
    }
}
