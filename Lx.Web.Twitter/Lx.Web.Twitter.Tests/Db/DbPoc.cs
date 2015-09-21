using System.Collections.Generic;
using Lx.Db.Protobuf;
using NUnit.Framework;
using ProtoBuf;

namespace Lx.Web.Twitter.Tests
{
    [TestFixture]
    class DbPoc
    {
        private const string DatabaseFile= "local.db";

/*        [Test]
        public void UsageTest()
        {
            UnqliteDB db = new UnqliteDB(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
            }
            db = new UnqliteDB(DatabaseFile);
            using (var session = db.OpenSession())
            {
                var retrieved = session.Get<MyContainer>("10");
                Assert.IsNotNull(retrieved);
                Assert.AreEqual("MyName", retrieved.Name);
                Assert.AreEqual(15, retrieved.Id);
                Assert.IsNotNull(retrieved.OtherIds);
                Assert.AreEqual(3, retrieved.OtherIds.Count);
            }
        }

        [Test]
        public void UsageFileTest()
        {
            using (var file = File.OpenWrite("local.db"))
            {
                FileDb db = new FileDb();
                db.Open(file);
                // No Support for OODB so...
            }
        }*/

        [Test]
        public void UsageDbTest()
        {            
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                var retrieved = session.Get<MyContainer>("10");
                Assert.IsNotNull(retrieved);
                Assert.AreEqual("MyName", retrieved.Name);
                Assert.AreEqual(15, retrieved.Id);
                Assert.IsNotNull(retrieved.OtherIds);
                Assert.AreEqual(3, retrieved.OtherIds.Count);
            }
            db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                var retrieved = session.Get<MyContainer>("10");
                Assert.IsNotNull(retrieved);
                Assert.AreEqual("MyName", retrieved.Name);
                Assert.AreEqual(15, retrieved.Id);
                Assert.IsNotNull(retrieved.OtherIds);
                Assert.AreEqual(3, retrieved.OtherIds.Count);
            }
        }
    }
    
    [ProtoBuf.ProtoContract]
    public class MyContainer
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public List<int> OtherIds { get; set; }
    }
}
