using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    public interface IFriendConnector
    {
        void BefriendWith(ILoggedUser user, IEnumerable<long> friends);
    }
}