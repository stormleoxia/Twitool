using System.Collections.Generic;
using System.Diagnostics;
using Lx.Db.Protobuf;
using NUnit.Framework;

namespace Lx.Web.Twitter.Tests.Db
{
    [TestFixture]
    public class DbPerformanceTest
    {
        private const string DatabaseFile = "current.db";

        [Test]
        public void UsageTest()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                for (int i = 0; i < 50000; ++i)
                {
                    session.Save(i, new MyContainer
                    {
                        Name = "MyName",
                        Id = i,
                        OtherIds = new List<int> {1, 2, 3}
                    });
                }
            }
            watch.Stop();
            System.Console.WriteLine("Elapsed: " + watch.Elapsed);
        }
    }
}
