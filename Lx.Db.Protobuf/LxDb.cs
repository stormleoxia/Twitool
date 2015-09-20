
namespace Lx.Db.Protobuf
{
    public class LxDb
    {
        private readonly string _databaseFile;

        public LxDb(string databaseFile)
        {
            _databaseFile = databaseFile;
        }

        public IDbSession OpenSession()
        {
            return new DbSession(_databaseFile);
        }
    }
}