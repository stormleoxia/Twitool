using System.Collections.Generic;
using ProtoBuf;

namespace Lx.Web.Twitter.Console
{
    [ProtoContract]
    public class FollowersTracker
    {
        public FollowersTracker()
        {
            Trackers = new List<FollowerTracker>();
        }

        /// <summary>
        /// Gets or sets the identifier of the followed.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ProtoMember(1)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the tracking information for all its followers.
        /// </summary>
        /// <value>
        /// The trackers.
        /// </value>
        [ProtoMember(2)]
        public List<FollowerTracker> Trackers { get; set; }
    }
}