using System;
using System.Collections.Generic;
using System.Linq;
using Lx.Db.Protobuf;

namespace Lx.Web.Twitter.Console
{
    internal sealed class FollowerCache : IFollowerCache, IDisposable
    {
        private LxDb _database;
        private readonly IDbSession _session;

        public FollowerCache()
        {
            _database = new LxDb("local.db");
            _session = _database.OpenSession();
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
}