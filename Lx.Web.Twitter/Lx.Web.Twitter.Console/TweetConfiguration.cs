using Tweetinvi;
using Tweetinvi.Core;

namespace Lx.Web.Twitter.Console
{
    class TweetConfiguration : ITweetConfiguration
    {
        public void ConfigureExceptionHandling()
        {
            ExceptionHandler.SwallowWebExceptions = false;
        }

        public void ConfigureRateLimiting()
        {
            // Use Auto limiter after connection (otherwise it will crash:
            // => Attempt to retrieve a Rate limit for null key)
            RateLimit.RateLimitTrackerOption = RateLimitTrackerOptions.TrackAndAwait;
        }
    }
}