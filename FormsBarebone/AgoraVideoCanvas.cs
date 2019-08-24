using System;
using Xamarin.Forms;

namespace FormsBarebone
{
    public class AgoraVideoCanvas : View
    {
        public static readonly BindableProperty UidProperty = BindableProperty.Create(nameof(Uid),typeof(int),typeof(AgoraVideoCanvas), (int)0);

        public int Uid
        {
            get { return (int)GetValue(UidProperty); }
            set { SetValue(UidProperty, value); }
        }
    }

    public interface IAgoraEngine
    {
        void InitAndJoinChannel(string channel, string token, string appId);
        event Action JoinedChannelEvent;
        event Action LeftChannelEvent;
        event RemoteUserJoinedEventType RemoteUserJoinedChannelEvent;
        event RemoteUserJoinedEventType RemoteUserLeftChannelEvent;
    }

    public delegate void RemoteUserJoinedEventType(int uid, int elapsed);
}
