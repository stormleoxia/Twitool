using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    public interface IFollowerCache : IDisposable
    {
        IEnumerable<long> GetFollowers(long follower);
        IEnumerable<long> SelectFollowersNotFollowed(long referenceUserId, long followerOwnerId);
        void FlagAsFollowed(long friend);
        void SetUser(ILoggedUser user);
    }
}