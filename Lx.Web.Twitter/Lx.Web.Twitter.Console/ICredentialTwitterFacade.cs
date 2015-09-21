using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    public interface ICredentialTwitterFacade
    {
        ITwitterCredentials AskForTwitterCredentials();
        void SetUserCredentials(ITwitterCredentials credentials);
    }
}