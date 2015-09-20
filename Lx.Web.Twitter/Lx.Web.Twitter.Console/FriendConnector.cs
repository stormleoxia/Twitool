using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    class FriendConnector : IFriendConnector
    {
        private readonly IFollowerCache _cache;

        public FriendConnector(IFollowerCache cache)
        {
            _cache = cache;
        }

        public void BefriendWith(ILoggedUser user, IEnumerable<long> friends)
        {
            foreach (var friend in friends)
            {
                user.FollowUser(friend);
                _cache.FlagAsFollowed(friend);
            }
        }
    }
}
