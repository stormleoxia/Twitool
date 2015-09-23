using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Lx.Web.Twitter.Console.Followers
{
    [ProtoContract]
    public class UserTracker
    {
        public UserTracker()
        {
            Subscribers = new List<long>();
            Subscriptions = new List<long>();
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
        /// Gets or sets the user ids of followers
        /// </summary>
        /// <value>
        /// The trackers.
        /// </value>
        [ProtoMember(2)]
        public List<long> Subscribers { get; set; }

        /// <summary>
        /// Gets or sets the user ids of whom he is following
        /// </summary>
        /// <value>
        /// The trackers.
        /// </value>
        [ProtoMember(3)]
        public List<long> Subscriptions { get; set; }


        /// <summary>
        /// Gets or sets the last load.
        /// </summary>
        /// <value>
        /// The last load.
        /// </value>
        [ProtoMember(4)]
        public DateTime LastLoad { get; set; }

        /// <summary>
        /// Gets of sets the user which were unsubscribed (from subscriptions)
        /// </summary>
        /// <value>
        /// The unsubscribed.
        /// </value>
        [ProtoMember(5)]
        public List<long> Unsubscribed { get; set; }
    }
}