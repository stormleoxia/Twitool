using System.Collections.Generic;
using Tweetinvi;
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
                var localFriend = friend;
                Auth.ExecuteOperationWithCredentials(user.Credentials, () => 
                user.FollowUser(localFriend));
                _cache.FlagAsFollowed(friend);
            }
        }
    }
}
