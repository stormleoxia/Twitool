using System.Collections.Generic;
using Microsoft.Runtime.CompilerServices;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Factories.User;

namespace Lx.Web.Twitter.Console
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
            IEnumerable<long> myFollowers = _cache.GetFollowers(user.Id, () => 
                Auth.ExecuteOperationWithCredentials(user.Credentials, () => user.GetFollowerIds()));
            List<long> list = new List<long>();
            foreach (var follower in myFollowers)
            {
                var localFollower = follower;
                var friendFollowers = _cache.GetFollowers(follower, () => 
                    Auth.ExecuteOperationWithCredentials(user.Credentials, () => User.GetFollowerIds(localFollower)));
                list.AddRange(friendFollowers);
            }
            return list;
        }
    }
}
