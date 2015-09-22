using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    class FriendConnector : IFriendConnector
    {
        private readonly IFollowerCache _cache;
        private readonly IConsole _console;

        public FriendConnector(IFollowerCache cache, IConsole console)
        {
            _cache = cache;
            _console = console;
        }

        public void BefriendWith(ILoggedUser user, IEnumerable<long> friends)
        {
            Exception lastException = null;
            foreach (var friend in friends)
            {
                var localFriend = friend;
                try
                {
                    Auth.ExecuteOperationWithCredentials(user.Credentials, () =>
                        user.FollowUser(localFriend));
                }
                catch (Exception e)
                {
                    lastException = e;
                }
                _cache.FlagAsFollowed(friend);
            }
            if (lastException != null)
            {
                _console.WriteLine(lastException.ToString());
            }
        }
    }
}
