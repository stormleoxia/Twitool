namespace Lx.Web.Twitter.Console
{
    public interface IConsole : IWriter
    {
        string ReadLine();
        IWriter Error { get; }
    }

    public interface IWriter
    {
        void WriteLine(string format, params object[] parameters);
    }
}