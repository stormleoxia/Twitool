using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using Lx.Web.Twitter.Console.Authentication;
using Lx.Web.Twitter.Console.Followers;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;

namespace Lx.Web.Twitter.Console
{
    public class TwitterMain
    {
        private readonly ICredentialManager _credManager;
        private readonly ILoginManager _loginManager;
        private readonly IFriendFinder _friendFinder;
        private readonly IFriendConnector _friendConnector;
        private readonly IConsole _console;
        private readonly ITweetConfiguration _configuration;
        private readonly IFollowerCache _cache;

        public TwitterMain(ICredentialManager credManager,
            ILoginManager loginManager,
            IFriendFinder friendFinder,
            IFriendConnector friendConnector,
            IConsole console,
            ITweetConfiguration configuration,
            IFollowerCache cache)
        {
            _credManager = credManager;
            _loginManager = loginManager;
            _friendFinder = friendFinder;
            _friendConnector = friendConnector;
            _console = console;
            _configuration = configuration;
            _cache = cache;
        }

        public void Run(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            _configuration.ConfigureExceptionHandling();
            try
            {
                var credentials = RetrieveCredentials();
                var connection = Login(credentials);
                _configuration.ConfigureRateLimiting();
                var friends = GetPotentialFriends(connection, 100);
                BefriendWith(connection, friends);
            }
            catch (Exception e)
            {
                _console.WriteLine(e.ToString());
            }
            finally
            {
                _cache.Dispose();
            }
            if (Debugger.IsAttached)
            {
                _console.WriteLine("Press a key to exit...");
                _console.ReadLine();
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _console.WriteLine(e.ToString());
        }


        private ITwitterCredentials RetrieveCredentials()
        {
            return _credManager.RetrieveCredentials();
        }

        private ILoggedUser Login(ITwitterCredentials credentials)
        {
            return _loginManager.Login(credentials);
        }

        private IEnumerable<long> GetPotentialFriends(ILoggedUser connection, int maxFriends)
        {
            return _friendFinder.GetPotentialFriends(connection, maxFriends);
        }
        
        private void BefriendWith(ILoggedUser user, IEnumerable<long> friends)
        {
            _friendConnector.BefriendWith(user, friends);
        }

    }

    public interface ITweetConfiguration
    {
        void ConfigureExceptionHandling();
        void ConfigureRateLimiting();
    }

    class TweetConfiguration : ITweetConfiguration
    {
        public void ConfigureExceptionHandling()
        {
            ExceptionHandler.SwallowWebExceptions = true;
        }

        public void ConfigureRateLimiting()
        {
            // Use Auto limiter after connection (otherwise it will crash:
            // => Attempt to retrieve a Rate limit for null key)
            RateLimit.RateLimitTrackerOption = RateLimitTrackerOptions.TrackAndAwait;
        }
    }
}