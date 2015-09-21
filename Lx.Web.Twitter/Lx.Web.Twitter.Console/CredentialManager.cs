using System;
using System.IO;
using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    public class CredentialManager : ICredentialManager
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConsole _console;
        private const string CredentialBase = "Lx.Twitter/credentials";

        public CredentialManager(IFileSystem fileSystem, IConsole console)
        {
            _fileSystem = fileSystem;
            _console = console;
        }

        /// <summary>
        /// Retrieves the credentials on local machine,
        /// If not available, ask for them.
        /// </summary>
        /// <returns></returns>
        public ITwitterCredentials RetrieveCredentials()
        {
            ITwitterCredentials credentials;
            var credentialsRetrieved = TryGetStoredCredentialsOnLocalMachine(out credentials);
            while (!credentialsRetrieved) // No Credentials on local machine, ask for them
            {
                credentials = AskForCredentials();
                credentialsRetrieved = ValidateCredentials(credentials);
            }
            StoreCredentialsOnLocalMachine(credentials);
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.AuthorizationKey, credentials.AuthorizationSecret);
            return credentials;
        }

        private void StoreCredentialsOnLocalMachine(ITwitterCredentials credentials)
        {
            
        }

        private bool ValidateCredentials(ITwitterCredentials credentials)
        {
            return true;
        }

        private ITwitterCredentials AskForCredentials()
        {
            var applicationCredentials = new ConsumerCredentials("consumer_key", "consumer_secret");
            var url = CredentialsCreator.GetAuthorizationURL(applicationCredentials);
            _console.WriteLine("Navigate to : {0}", url);
            _console.WriteLine("Enter the captcha: ");
            var captcha = _console.ReadLine();
            var newCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, applicationCredentials);
            if (newCredentials == null)
            {
                _console.Error.WriteLine("Bad captcha");
            }
            else
            {
                _console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
                _console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);
            }
            return newCredentials;
        }

        private bool TryGetStoredCredentialsOnLocalMachine(out ITwitterCredentials credentials)
        {
            credentials = null;
            var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var credentialFileName = Path.Combine(appPath, CredentialBase);
            if (!_fileSystem.FileExists(credentialFileName))
            {
                return false;
            }
            using (var reader = _fileSystem.Open(credentialFileName))
            {
                
            }
            return false;
        }
    }

    public interface IFileSystem
    {
        bool FileExists(string filePath);
        IDisposable Open(string filePath);
    }

    public interface IConsole
    {
        void WriteLine(string format, params object[] parameters);
        string ReadLine();
        IConsole Error { get; }
    }
}