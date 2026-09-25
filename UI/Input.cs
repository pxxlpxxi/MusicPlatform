using System;

namespace MusicPlatform.UI
{
    public class Input : IInput
    {
        private readonly IOutput _output = new Output();
        private bool _quitRequested;
        public bool QuitRequested => _quitRequested;
        public string ReadString()
        {
            return Console.ReadLine() ?? "";
        }

        public string ReadRequiredString()
        {
            while (true)
            {
                string input = ReadString();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
            }
        }

        public int ReadInt()
        {
            while (true)
            {
                string input = ReadString();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }
            }
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
        public ConsoleKey WaitForKeyOrQuit()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q)
                {
                    _quitRequested = true;
                    return ConsoleKey.Q;
                }
                return key.Key;
            }
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            throw new NotImplementedException();
        }
    }

}

