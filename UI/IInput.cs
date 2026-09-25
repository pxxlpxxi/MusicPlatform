namespace MusicPlatform.UI
{
    public interface IInput
    {
        string ReadString();
        string ReadRequiredString();
        int ReadInt();
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);

        ConsoleKey WaitForKeyOrQuit();
    }
}
