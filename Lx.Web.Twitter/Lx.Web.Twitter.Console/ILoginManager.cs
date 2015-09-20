using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    public interface ILoginManager
    {
        ILoggedUser Login(ITwitterCredentials credentials);
    }
}