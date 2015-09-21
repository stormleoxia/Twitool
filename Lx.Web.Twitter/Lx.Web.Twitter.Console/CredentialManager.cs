using System;
using System.IO;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    public class CredentialManager : ICredentialManager
    {
        private const string CredentialBase = "Lx.Twitter/credentials";

        public CredentialManager()
        {

        }

        public ITwitterCredentials RetrieveCredentials()
        {
            ITwitterCredentials credentials;
            var credentialsRetrieved = TryGetCredentials(out credentials);
            while (!credentialsRetrieved)
            {
                AskForCredentials(out credentials);
                credentialsRetrieved = ValidateCredentials(credentials);
            }
            return credentials;
        }

        private bool ValidateCredentials(ITwitterCredentials credentials)
        {
            return true;
        }

        private void AskForCredentials(out ITwitterCredentials credentials)
        {
            credentials = new TwitterCredentials();
        }

        private bool TryGetCredentials(out ITwitterCredentials credentials)
        {
            credentials = new TwitterCredentials
            {
                ConsumerKey = "",
                ConsumerSecret = "",
                AuthorizationKey = "",
                AuthorizationSecret = "",
            };
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var credentialFileName = Path.Combine(appPath, CredentialBase);
/*            if (!_fileSystem.FileExists(credentialFileName))
            {
                return false;
            }
            using (var reader = _fileSystem.OpenText(credentialFileName))
            {
                var result = reader.ReadToEnd();

            }*/
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.AuthorizationKey, credentials.AuthorizationSecret);
            return true;
        }
    }
}