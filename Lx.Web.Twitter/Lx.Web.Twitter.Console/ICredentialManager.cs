using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    public interface ICredentialManager
    {
        //Credentials RetrieveCredentials();
        ITwitterCredentials RetrieveCredentials();
    }
}