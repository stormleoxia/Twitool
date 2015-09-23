using System;
using System.Collections.Generic;
using System.Linq;
using Lx.Db.Protobuf;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console.Followers
{
    internal sealed class FollowerCache : IFollowerCache
    {
        private LxDb _database;
        private readonly IDbSession _session;
        private ILoggedUser _user;

        public FollowerCache(IConsole console)
        {
            _database = new LxDb("local.db");
            _session = _database.OpenSession();
            var users = _session.GetAll<UserTracker>();
            console.WriteLine("Users in database: " + users.Count());
        }

        public IEnumerable<long> GetFollowers(long id)
        {
            var userTracker = GetOrLoadUserTracker(id);
            return userTracker.Subscribers.ToArray();
        }

        private UserTracker GetOrLoadUserTracker(long id)
        {
            var userTracker = _session.Get<UserTracker>(id);
            if (UserNotLoadedOrTooOld(userTracker))
            {
                userTracker = LoadAndSaveTracker(id);
            }
            return userTracker;
        }

        /// <summary>
        /// Check if user is not loaded or loaded but a long time ago (30 days).
        /// </summary>
        /// <param name="userTracker">The user tracker.</param>
        /// <returns></returns>
        private bool UserNotLoadedOrTooOld(UserTracker userTracker)
        {
            return userTracker == null || DateTime.UtcNow - userTracker.LastLoad > TimeSpan.FromDays(30);
        }

        /// <summary>
        /// Selects only the followers from follower owner which not followed already by reference user.
        /// </summary>
        /// <param name="referenceUserId">The reference user identifier.</param>
        /// <param name="followerOwnerId">The follower owner identifier.</param>
        /// <returns></returns>
        public IEnumerable<long> SelectFollowersNotFollowed(long referenceUserId, long followerOwnerId)
        {
            var referenceUser = GetOrLoadUserTracker(referenceUserId);
            var followerOwner = GetOrLoadUserTracker(followerOwnerId);            
            var subscriptions = new HashSet<long>(referenceUser.Subscriptions);
            var oldSubscriptions = new HashSet<long>(referenceUser.OldSubscriptions);
            var unsubcriptions = new HashSet<long>(referenceUser.Unsubscribed);
            return followerOwner.Subscribers.Where(
                x => !subscriptions.Contains(x) && 
                !unsubcriptions.Contains(x) &&
                oldSubscriptions.Contains(x));
        }

        public IEnumerable<long> SelectSubscriptionsNotFollowing(long userId)
        {
            var referenceUser = LoadAndSaveTracker(userId); // Force a up-to-date load
            var followers = new HashSet<long>(referenceUser.Subscribers);
            var unsubcriptions = new HashSet<long>(referenceUser.Unsubscribed);
            return referenceUser.Subscriptions.Where(x => !unsubcriptions.Contains(x) && !followers.Contains(x));
        }

        private UserTracker LoadAndSaveTracker(long id)
        {
            var userTracker = LoadUserTracker(id);
            _session.Save(id, userTracker);
            _session.Commit();
            return userTracker;
        }

        private UserTracker LoadUserTracker(long id)
        {
            var userTracker = new UserTracker {Id = id};
            userTracker.Subscribers = Secured(() => User.GetFollowerIds(id)).ToList();
            userTracker.Subscriptions = Secured(() => User.GetFriendIds(id)).ToList();
            var oldVersion = _session.Get<UserTracker>(id);
            if (oldVersion != null)
            {
                userTracker.Unsubscribed = oldVersion.Unsubscribed;
                userTracker.OldSubscriptions.AddRange(oldVersion.Subscriptions);                
            }
            userTracker.LastLoad = DateTime.UtcNow;
            return userTracker;
        }

        private T Secured<T>(Func<T> func)
        {
            return Auth.ExecuteOperationWithCredentials(_user.Credentials, func);
        }


        public void FlagAsFollowed(long userId)
        {
            var mainUserTracker = _session.Get<UserTracker>(_user.Id);
            mainUserTracker.Subscriptions.Add(userId);
            _session.Save(_user.Id, mainUserTracker);
        }

        public void SetUser(ILoggedUser user)
        {
            _user = user;
        }

        public void FlagAsUnFollowed(long userId)
        {
            var mainUserTracker = _session.Get<UserTracker>(_user.Id);
            mainUserTracker.Subscriptions.Remove(userId);
            mainUserTracker.Unsubscribed.Add(userId);
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}