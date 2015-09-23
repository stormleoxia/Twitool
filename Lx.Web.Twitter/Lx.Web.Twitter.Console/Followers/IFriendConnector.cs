using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    public interface IFriendConnector
    {
        void BefriendWith(ILoggedUser user, IEnumerable<long> friends);
        void UnFriendWith(ILoggedUser user, IEnumerable<long> noLongerFriends);
    }
}