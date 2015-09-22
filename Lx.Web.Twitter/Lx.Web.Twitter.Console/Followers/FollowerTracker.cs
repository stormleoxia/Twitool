using ProtoBuf;

namespace Lx.Web.Twitter.Console
{
    [ProtoContract]
    public class FollowerTracker
    {        
        /// <summary>
        /// Gets or sets the identifier of the follower.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ProtoMember(1)]
        public long Id { get; set; }        

        /// <summary>
        /// Gets or sets a value indicating whether the follower referenced by <see cref="FollowerTracker"/> is followed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if followed; otherwise, <c>false</c>.
        /// </value>
        [ProtoMember(2)]
        public bool Followed { get; set; }
    }
}