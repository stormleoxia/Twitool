using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    internal class FriendFinder : IFriendFinder
    {
        private readonly IFollowerCache _cache;
        private readonly IConsole _console;

        public FriendFinder(IFollowerCache cache, IConsole console)
        {
            _cache = cache;
            _console = console;
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
            }
            _console.WriteLine("Found potential {0} friends", list.Count);
            return list;
        }

        /// <summary>
        /// Since there were lots of friends added, we need to check which of them returned the favor 
        /// and subscribe to us.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="maxFriends">The maximum friends.</param>
        /// <returns></returns>
        public IEnumerable<long> GetNotFriends(ILoggedUser user, int maxFriends)
        {
            _cache.SetUser(user);
            List<long> list = _cache.SelectSubscriptionsNotFollowing(user.Id).ToList();
            _console.WriteLine("Found potential {0} unfriends", list.Count);
            return list;
        }
 
    }
}
