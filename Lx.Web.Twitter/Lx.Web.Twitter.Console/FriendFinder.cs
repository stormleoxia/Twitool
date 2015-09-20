using System;
using System.Collections.Generic;
using System.Linq;
using Lx.Db.Protobuf;
using ProtoBuf;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

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
            IEnumerable<long> myFollowers = _cache.GetFollowers(() => user.GetFollowerIds());
            List<long> list = new List<long>();
            foreach (var follower in myFollowers)
            {
                var localFollower = follower;
                var friendFollowers = _cache.GetFollowers(follower, () => User.GetFollowerIds(localFollower));
                list.AddRange(friendFollowers);
            }
            return list;
        }
    }

    internal interface IFollowerCache
    {
        IEnumerable<long> GetFollowers(Func<IEnumerable<long>> load);
        IEnumerable<long> GetFollowers(long follower, Func<IEnumerable<long>> load);
        void FlagAsFollowed(long friend);
    }

    internal sealed class FollowerCache : IFollowerCache, IDisposable
    {
        private LxDb _database;
        private readonly IDbSession _session;

        public FollowerCache()
        {
            _database = new LxDb("local.db");
            _session = _database.OpenSession();
        }

        public IEnumerable<long> GetFollowers(Func<IEnumerable<long>> load)
        {
            var id = User.GetLoggedUser().Id;
            return GetFollowers(id, load);
        }

        private List<long> GetValidTrackers(FollowersTracker followerTrackers)
        {
            return followerTrackers.Trackers.Where(x => !x.Followed).Select(x => x.Id).ToList();
        }

        public IEnumerable<long> GetFollowers(long id, Func<IEnumerable<long>> load)
        {
            var followerTrackers = _session.Get<FollowersTracker>(id);
            if (followerTrackers == null)
            {
                followerTrackers = new FollowersTracker{Id = id};
            }
            var valids = GetValidTrackers(followerTrackers);
            if (valids.Count == 0 || valids.Count < 150)
            {
                var newIds = load();
                foreach (var newId in newIds)
                {
                    var tracker = new FollowerTracker {Followed = false, Id = newId};
                    followerTrackers.Trackers.Add(tracker);
                }
                _session.Save(id, followerTrackers);
                valids = GetValidTrackers(followerTrackers);
            }
            return valids;
        }

        public void FlagAsFollowed(long userId)
        {
            var allTracked = _session.GetAll<FollowersTracker>();
            foreach (var follower in allTracked)
            {
                foreach (var tracker in follower.Trackers)
                {
                    if (tracker.Id == userId)
                    {
                        tracker.Followed = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }

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
