using System;
using System.IO;
using ProtoBuf;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    public class CredentialManager : ICredentialManager
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICredentialTwitterFacade _credentialTwitterFacade;
        private static string _credentialFileName;
        private const string CredentialBase = "Lx.Twitter/credentials";

        public CredentialManager(IFileSystem fileSystem, ICredentialTwitterFacade credentialTwitterFacade)
        {
            _fileSystem = fileSystem;
            _credentialTwitterFacade = credentialTwitterFacade;
        }

        /// <summary>
        /// Retrieves the credentials on local machine,
        /// If not available, ask for them.
        /// </summary>
        /// <returns></returns>
        public ITwitterCredentials RetrieveCredentials()
        {
            ITwitterCredentials credentials = null;
            var areCredentialsStored = AreCredentialsStored();
            var credentialsRetrieved = false;
            if (areCredentialsStored)
            {
                credentialsRetrieved = TryGetStoredCredentialsOnLocalMachine(out credentials);
            }
            while (!credentialsRetrieved) // No Credentials on local machine, ask for them
            {
                credentials = AskForCredentials();
                credentialsRetrieved = ValidateCredentials(credentials);
            }
            if (!areCredentialsStored)
            {
                StoreCredentialsOnLocalMachine(credentials);
            }
            _credentialTwitterFacade.SetUserCredentials(credentials);
            return credentials;
        }

        private void StoreCredentialsOnLocalMachine(ITwitterCredentials credentials)
        {
            var serializable = new SerializedCredentials
            {
                AuthorizationKey = credentials.AuthorizationKey,
                AuthorizationSecret = credentials.AuthorizationSecret,
                ConsumerKey = credentials.ConsumerKey,
                ConsumerSecret = credentials.ConsumerSecret
            };
            using (var reader = _fileSystem.Open(CredentialFileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Serializer.Serialize(reader.Stream, serializable);
            }
        }

        private bool ValidateCredentials(ITwitterCredentials credentials)
        {
            return true;
        }

        private ITwitterCredentials AskForCredentials()
        {
            var newCredentials = _credentialTwitterFacade.AskForTwitterCredentials();
            return newCredentials;
        }

        private bool TryGetStoredCredentialsOnLocalMachine(out ITwitterCredentials credentials)
        {
            credentials = null;
            using (var reader = _fileSystem.Open(CredentialFileName, FileMode.Open, FileAccess.Read))
            {
                var cred = Serializer.Deserialize<SerializedCredentials>(reader.Stream);
                if (cred != null)
                {
                    credentials = new TwitterCredentials(cred.ConsumerKey, cred.ConsumerSecret);
                    credentials.AuthorizationKey = cred.AuthorizationKey;
                    credentials.AuthorizationSecret = cred.AuthorizationSecret;
                    return true;
                }
            }
            return false;
        }

        private bool AreCredentialsStored()
        {
            var credentialFileName = CredentialFileName;
            return _fileSystem.FileExists(credentialFileName);
        }

        private static string CredentialFileName
        {
            get
            {
                if (_credentialFileName == null)
                {
                    var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    _credentialFileName = Path.Combine(appPath, CredentialBase);
                }
                return _credentialFileName;
            }
        }
    }
}