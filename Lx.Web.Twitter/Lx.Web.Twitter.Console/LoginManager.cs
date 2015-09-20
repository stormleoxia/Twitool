using System;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    public class LoginManager : ILoginManager
    {
        public ILoggedUser Login(ITwitterCredentials credentials)
        {
            return User.GetLoggedUser(credentials);
        }
    }
}
