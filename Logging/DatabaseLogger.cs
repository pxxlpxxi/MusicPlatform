namespace MusicPlatform.Logging
    internal static class DatabaseLogger
{
    private const string LogFile =
        "database.log";

    private static readonly object _lock =
        new();


    internal static void Log(
        string operation,
        string entity,
        string details)
    {
        string timestamp =
            DateTime.Now.ToString(
                "yyyy-MM-dd HH:mm:ss");

        string logEntry =
            $"{timestamp} | {operation} | {entity} | {details}{Environment.NewLine}";


        lock (_lock)
        {
            const int maxAttempts = 5;

            for (
                int attempt = 1;
                attempt <= maxAttempts;
                attempt++)
            {
                try
                {
                    using FileStream stream =
                        new FileStream(
                            LogFile,
                            FileMode.Append,
                            FileAccess.Write,
                            FileShare.ReadWrite);

                    using StreamWriter writer =
                        new StreamWriter(stream);

                    writer.Write(
                        logEntry);

                    return;
                }
                catch (IOException)
                {
                    if (
                        attempt ==
                        maxAttempts)
                    {
                        throw;
                    }

                    //wait for a short period before retrying
                    //wait time increases with each attempt to reduce the chance of collision
                    //50ms, 100ms, 150ms, 200ms, then give up
                    Thread.Sleep(50 * attempt);
                }
            }
        }
    }
}
}

//namespace MusicPlatform.Logging
//{
//    internal static class DatabaseLogger
//    {
//        private const string LogFile = "database.log"; // MusicPlatform\bin\Debug\net10.0\database.log, MusicPlatform.WebApi\database.log

//        private static readonly object _lock = new();

//        internal static void Log(
//            string operation,
//            string entity,
//            string details)
//        {
//            string timestamp =
//                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

//            string logEntry =
//                $"{timestamp} | {operation} | {entity} | {details}{Environment.NewLine}";

//            lock (_lock)
//            {
//                File.AppendAllText(
//                    LogFile,
//                    logEntry);
//            }
//        }
//    }
//}
////namespace MusicPlatform.Logging
////{
////    internal static class DatabaseLogger
////    {
////        private const string LogFile = "database.log"; // MusicPlatform\bin\Debug\net10.0\database.log

////        internal static void Log(string operation, string entity, string details)
////        {
////            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

////            string logEntry = $"{timestamp} | {operation} | {entity} | {details}{Environment.NewLine}";

////            File.AppendAllText(LogFile, logEntry);
////        }
////    }
////}
