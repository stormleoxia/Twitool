using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    internal class FriendFinder : IFriendFinder
    {
        private readonly IFollowerCache _cache;

        public FriendFinder(IFollowerCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets the potential friends.
        /// Potential friends are followers of your followers.
        /// However since some have lots of followers you cannot get all followers for one user with one shoot
        /// and you are not sure those followers were not already processed before (in this run or in a run before).
        /// So there is a need to keep track of followers exploration and there is a need for a intermediate cache between 
        /// twitter and the current call (avoid asking twitter for followers to manage each time).
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="maxFriends">The maximum friends.</param>
        /// <returns></returns>
        public IEnumerable<long> GetPotentialFriends(ILoggedUser user, int maxFriends)
        {
            _cache.SetUser(user);
            IEnumerable<long> myFollowers = _cache.GetFollowers(user.Id);
            List<long> list = new List<long>();
            foreach (var follower in myFollowers)
            {
                var friendFollowers = _cache.SelectFollowersNotFollowed(user.Id, follower).ToArray();
                list.AddRange(friendFollowers);
                if (friendFollowers.Length > 0)
                {
                    break;
                }
            }
            return list;
        }
    }
}
