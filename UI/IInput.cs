namespace MusicPlatform.UI
{
    public interface IInput
    {
        string ReadString(string text);

        string ReadRequiredString(string text);

        int ReadInt(string text);
    }
}
