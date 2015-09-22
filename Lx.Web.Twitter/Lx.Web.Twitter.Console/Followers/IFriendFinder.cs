using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    public interface IFriendFinder
    {
        IEnumerable<long> GetPotentialFriends(ILoggedUser user, int maxFriends);
    }
}