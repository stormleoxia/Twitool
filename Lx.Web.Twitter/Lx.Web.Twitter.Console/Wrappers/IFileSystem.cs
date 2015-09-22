using System;
using System.IO;

namespace Lx.Web.Twitter.Console
{
    public interface IFileSystem
    {
        bool FileExists(string filePath);
        IFileStream Open(string filePath, FileMode mode, FileAccess access);
    }

    public interface IFileStream : IDisposable
    {
        Stream Stream { get; }
    }
}