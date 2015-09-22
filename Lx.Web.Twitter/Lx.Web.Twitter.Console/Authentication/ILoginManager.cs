using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Authentication
{
    public interface ILoginManager
    {
        ILoggedUser Login(ITwitterCredentials credentials);
    }
}