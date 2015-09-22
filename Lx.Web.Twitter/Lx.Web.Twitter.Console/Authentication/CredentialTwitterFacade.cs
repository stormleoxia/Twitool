using Tweetinvi;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    class CredentialTwitterFacade : ICredentialTwitterFacade
    {
        private const string ConsumerKey = "UdfjkLKD4UfygStlFDAfLuH8F";
        private readonly IConsole _console;

        public CredentialTwitterFacade(IConsole console)
        {
            _console = console;
        }

        public ITwitterCredentials AskForTwitterCredentials()
        {
            _console.WriteLine("Please provide Consumer Secret: ");
            string consumerSecret = _console.ReadLine();
            var applicationCredentials = new TwitterCredentials(ConsumerKey, consumerSecret);
            _console.WriteLine("Please provide Access Token: ");
            applicationCredentials.AccessToken = _console.ReadLine();
            _console.WriteLine("Please provide Access Token Secret: ");
            applicationCredentials.AccessTokenSecret = _console.ReadLine();            
            return applicationCredentials;
        }

        public void SetUserCredentials(ITwitterCredentials credentials)
        {
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret, credentials.AuthorizationKey,
                credentials.AuthorizationSecret);
        }
    }
}