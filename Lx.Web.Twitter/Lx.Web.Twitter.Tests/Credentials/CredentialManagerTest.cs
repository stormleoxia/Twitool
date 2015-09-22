using System;
using System.IO;
using Autofac.Extras.Moq;
using Lx.Web.Twitter.Console;
using Lx.Web.Twitter.Console.Authentication;
using Moq;
using NUnit.Framework;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Tests.Credentials
{
    [TestFixture]
    class CredentialManagerTest 
    {
        [Test]
        public void UsageTest()
        {
            using (var mock = AutoMock.GetStrict())
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string filePath = Path.Combine(appData, @"Lx.Twitter/credentials");
                mock.Mock<IFileSystem>()
                    .Setup(x => x.FileExists(filePath))
                    .Returns(true);

                var fileStreamMock = new Mock<IFileStream>();
                fileStreamMock.Setup(x => x.Stream).Returns(new MemoryStream());

                mock.Mock<IFileSystem>()
                    .Setup(x => x.Open(filePath, FileMode.Open, FileAccess.Read))
                    .Returns(fileStreamMock.Object);
                ITwitterCredentials twitterCredentials = new TwitterCredentials();
                mock.Mock<ICredentialTwitterFacade>()
                    .Setup(x => x.AskForTwitterCredentials())
                    .Returns(twitterCredentials);
                mock.Mock<ICredentialTwitterFacade>()
                    .Setup(x => x.SetUserCredentials(It.IsAny<ITwitterCredentials>()));

                var cm = mock.Create<CredentialManager>();
                var credentials = cm.RetrieveCredentials();
                Assert.IsNotNull(credentials);
            }
        }
    }
}
