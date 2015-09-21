using System.IO;

namespace Lx.Web.Twitter.Console
{
    public class ConsoleWrapper : IConsole
    {
        private readonly TextWriter _writer;
        private readonly TextReader _reader;
        private readonly IWriter _error;

        public ConsoleWrapper()
        {
            _writer = System.Console.Out;
            _reader = System.Console.In;
            _error = new ConsoleWrapper(System.Console.Error);
        }

        private ConsoleWrapper(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteLine(string format, params object[] parameters)
        {
            _writer.WriteLine(format, parameters);
        }

        public string ReadLine()
        {
            return _reader.ReadLine();
        }

        public IWriter Error
        {
            get { return _error; }
        }
    }

}
