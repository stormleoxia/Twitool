using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console.Authentication
{
    public interface ICredentialManager
    {
        //Credentials RetrieveCredentials();
        ITwitterCredentials RetrieveCredentials();
    }
}