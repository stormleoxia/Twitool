using System;
using System.Collections.Generic;

namespace Lx.Web.Twitter.Console
{
    internal interface IFollowerCache
    {
        IEnumerable<long> GetFollowers(long follower, Func<IEnumerable<long>> load);
        void FlagAsFollowed(long friend);
    }
}