namespace Lx.Web.Twitter.Console
{
    public interface ITweetConfiguration
    {
        void ConfigureExceptionHandling();
        void ConfigureRateLimiting();
    }
}