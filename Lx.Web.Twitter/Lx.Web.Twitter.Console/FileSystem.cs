using System.IO;

namespace Lx.Web.Twitter.Console
{
    class FileSystem : IFileSystem
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public IFileStream Open(string filePath, FileMode mode, FileAccess access)
        {
            return new FileStreamWrapper(File.Open(filePath, mode, access));
        }
    }

    internal sealed class FileStreamWrapper : IFileStream
    {
        private readonly FileStream _stream;

        public FileStreamWrapper(FileStream stream)
        {
            _stream = stream;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public Stream Stream
        {
            get { return _stream; }
        }
    }
}
