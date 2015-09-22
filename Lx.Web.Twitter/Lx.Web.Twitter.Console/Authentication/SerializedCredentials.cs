using ProtoBuf;
using Tweetinvi.Core.Credentials;

namespace Lx.Web.Twitter.Console
{
    [ProtoContract]
    public class SerializedCredentials
    {
        [ProtoMember(1)]
        public string ConsumerKey { get; set; }

        [ProtoMember(2)]
        public string ConsumerSecret { get; set; }

        [ProtoMember(3)]
        public string AccessToken { get; set; }

        [ProtoMember(4)]
        public string AccessTokenSecret { get; set; }

    }
}
