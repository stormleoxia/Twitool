using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using Lx.Web.Twitter.Console.Authentication;
using Lx.Web.Twitter.Console.Followers;
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
        private Random _random;

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
            _random = new Random(Environment.TickCount);
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
                var unfriends = _friendFinder.GetNotFriends(connection, Randomize(100));
                _friendConnector.UnFriendWith(connection, unfriends);
                var friends = GetPotentialFriends(connection, Randomize(100));                
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

        private int Randomize(int range)
        {
            var add = _random.Next(0, 10);
            return Math.Max(5, range - 5 + add);
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
}