using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;
using User = Tweetinvi.User;

namespace Lx.Web.Twitter.Console.Authentication
{
    public class LoginManager : ILoginManager
    {
        public ILoggedUser Login(ITwitterCredentials credentials)
        {
            var loggedUser = User.GetLoggedUser(credentials);
            return loggedUser;
        }
    }
}
