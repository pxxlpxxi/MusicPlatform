namespace MusicPlatform.UI
{
    public class Output : IOutput
    {
        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
