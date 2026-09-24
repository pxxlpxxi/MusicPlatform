namespace MusicPlatform.UI
{
    public class Input : IInput
    {
        public string ReadString(string text)
        {
            Console.Write(text);
            return Console.ReadLine() ?? "";
        }

        public string ReadRequiredString(string text)
        {
            while (true)
            {
                Console.Write(text);

                string input = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("Input cannot be empty.");
            }
        }

        public int ReadInt(string text)
        {
            while (true)
            {
                Console.Write(text);

                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                Console.WriteLine("Please enter a valid number.");
            }
        }
    }
}
