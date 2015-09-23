using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    public interface IFollowerCache : IDisposable
    {
        void SetUser(ILoggedUser user);
        IEnumerable<long> GetFollowers(long follower);

        /// <summary>
        /// Selects the followers of owner not followed already by reference user.
        /// However it filters out followers already unsubscribed
        /// </summary>
        /// <param name="referenceUserId">The reference user identifier.</param>
        /// <param name="followerOwnerId">The follower owner identifier.</param>
        /// <returns></returns>
        IEnumerable<long> SelectFollowersNotFollowed(long referenceUserId, long followerOwnerId);

        /// <summary>
        /// Selects the subscriptions of user not following him.
        /// However it filters out followers already unsubscribed.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<long> SelectSubscriptionsNotFollowing(long userId);

        void FlagAsFollowed(long friend);
        void FlagAsUnFollowed(long friend);
    }
}