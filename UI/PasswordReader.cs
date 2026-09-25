using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlatform.UI
{
    internal class PasswordReader
    {
        private readonly IInput _input;
        private readonly IOutput _output;

        internal PasswordReader(IInput input, IOutput output) {
            _input = input;
            _output = output;
        }

        public string Read()
        {
            string password = "";
            ConsoleKeyInfo key;
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;

            do
            {
                key = _input.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    _output.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    _output.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.ForegroundColor = originalColor;
            _output.WriteLine("");
            return password;
        }
    }
}
