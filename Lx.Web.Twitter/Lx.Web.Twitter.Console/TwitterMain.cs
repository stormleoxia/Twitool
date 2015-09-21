using System.Collections;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    public class TwitterMain
    {
        private readonly ICredentialManager _credManager;
        private readonly ILoginManager _loginManager;
        private readonly IFriendFinder _friendFinder;
        private readonly IFriendConnector _friendConnector;

        public TwitterMain(ICredentialManager credManager,
            ILoginManager loginManager,
            IFriendFinder friendFinder,
            IFriendConnector friendConnector)
        {
            _credManager = credManager;
            _loginManager = loginManager;
            _friendFinder = friendFinder;
            _friendConnector = friendConnector;
        }

        public void Run(string[] args)
        {
            var credentials = RetrieveCredentials();
            var connection = Login(credentials);
            // Use Auto limiter after connection (otherwise it will crash:
            // => Attempt to retrieve a Rate limit for null key)
            RateLimit.RateLimitTrackerOption = RateLimitTrackerOptions.TrackAndAwait;
            var friends = GetPotentialFriends(connection, 100);
            BefriendWith(connection, friends);     
        }



        private ITwitterCredentials RetrieveCredentials()
        {
            return _credManager.RetrieveCredentials();
        }

        private ILoggedUser Login(ITwitterCredentials credentials)
        {
            return _loginManager.Login(credentials);
        }

        private IEnumerable<long> GetPotentialFriends(ILoggedUser connection, int maxFriends)
        {
            return _friendFinder.GetPotentialFriends(connection, maxFriends);
        }
        
        private void BefriendWith(ILoggedUser user, IEnumerable<long> friends)
        {
            _friendConnector.BefriendWith(user, friends);
        }

    }
}