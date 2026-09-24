namespace MusicPlatform.UI
{
    public class Input : IInput
    {
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
    }
}
