using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    class CredentialTwitterFacade : ICredentialTwitterFacade
    {
        private readonly IConsole _console;

        public CredentialTwitterFacade(IConsole console)
        {
            _console = console;
        }

        public ITwitterCredentials AskForTwitterCredentials()
        {
            var applicationCredentials = new TwitterCredentials("consumer_key", "consumer_secret");
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

        public void SetUserCredentials(ITwitterCredentials credentials)
        {
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.AuthorizationKey,
                credentials.AuthorizationSecret);
        }
    }
}