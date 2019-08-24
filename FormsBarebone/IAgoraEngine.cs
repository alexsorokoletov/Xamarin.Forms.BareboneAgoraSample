using System;

namespace FormsBarebone
{
    public delegate void RemoteUserJoinedEventType(int uid, int elapsed);

    public interface IAgoraEngine
    {
        void InitAndJoinChannel(string channel, string token, string appId);
        event Action JoinedChannelEvent;
        event Action LeftChannelEvent;
        event RemoteUserJoinedEventType RemoteUserJoinedChannelEvent;
        event RemoteUserJoinedEventType RemoteUserLeftChannelEvent;
    }
}
