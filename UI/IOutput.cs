namespace MusicPlatform.UI
{
    public interface IOutput
    {
        void Write(string message);

        void WriteLine(string message);

        void WriteSuccess(string message);

        void WriteError(string message);

        void WriteInfo(string message);
    }
}
