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
            var number = 0;
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
                if (number++ % 100 == 0)
                {
                    _console.WriteLine("Added {0} friends", number);
                }
            }
            _console.WriteLine("Added {0} friends", number);
            DisplayException(lastException);
        }

        private void DisplayException(Exception lastException)
        {
            if (lastException != null)
            {
                _console.WriteLine(lastException.ToString());
            }
        }

        public void UnFriendWith(ILoggedUser user, IEnumerable<long> noLongerFriends)
        {
            Exception lastException = null;
            var number = 0;
            foreach (var friend in noLongerFriends)
            {
                var localFriend = friend;
                try
                {
                    Auth.ExecuteOperationWithCredentials(user.Credentials, () =>
                        user.UnFollowUser(localFriend));
                }
                catch (Exception e)
                {
                    lastException = e;
                }
                _cache.FlagAsUnFollowed(friend);
                if (number++ % 100 == 0)
                {
                    _console.WriteLine("Removed {0} friends", number);
                }
            }
            _console.WriteLine("Removed {0} friends", number);
            DisplayException(lastException);
        }
    }
}
