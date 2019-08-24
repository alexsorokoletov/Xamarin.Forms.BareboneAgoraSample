using System;
using FormsBarebone;
using FormsBarebone.Droid;
using Xamarin.Forms;
using DT.Xamarin.Agora;

[assembly: Dependency(typeof(AgoraEngine))]
namespace FormsBarebone.Droid
{
    public class AgoraEngine : IRtcEngineEventHandler, IAgoraEngine
    {
        private DT.Xamarin.Agora.RtcEngine _rtcEngine;
        internal RtcEngine RtcEngine => _rtcEngine;

        public event Action JoinedChannelEvent;
        public event Action LeftChannelEvent;
        public event RemoteUserJoinedEventType RemoteUserJoinedChannelEvent;
        public event RemoteUserJoinedEventType RemoteUserLeftChannelEvent;
        public event RemoteUserJoinedEventType FirstVideoDecodedEvent;

        public AgoraEngine()
        {
            DemoHelper.CurrentAgoraEngine = this;
        }

        public void InitAndJoinChannel(string channel, string token, string appId)
        {
            //following this https://docs.agora.io/en/Video/initialize_android_video?platform=Android
            _rtcEngine = RtcEngine.Create(DemoHelper.Context, appId, this);
            _rtcEngine.DisableVideo();
            _rtcEngine.DisableAudio();
            _rtcEngine.SetChannelProfile(Constants.ChannelProfileCommunication); //Or Constants.ChannelProfileLiveBroadcasting
            const string extraData = null;
            const int requestSpecificUserId = 0; // this should be same as in the token
            var joinResult = _rtcEngine.JoinChannel(token, channel, extraData, requestSpecificUserId);
            //lookup values of the result here: https://docs.agora.io/en/Video/API%20Reference/java/classio_1_1agora_1_1rtc_1_1_i_rtc_engine_event_handler_1_1_error_code.html
        }

        public override void OnError(int errorCode)
        {
            //lookup errors here https://docs.agora.io/en/Video/API%20Reference/java/classio_1_1agora_1_1rtc_1_1_i_rtc_engine_event_handler_1_1_error_code.html
            Console.WriteLine($"Agora error {errorCode}");
        }

        /// <summary>
        /// https://docs.agora.io/en/Video/API%20Reference/java/classio_1_1agora_1_1rtc_1_1_i_rtc_engine_event_handler.html#aa466d599b13768248ac5febd2978c2d3
        /// </summary>
        public override void OnUserJoined(int uid, int elapsed)
        {
            Console.WriteLine($"User {uid} joined the channel");
            RemoteUserJoinedChannelEvent?.Invoke(uid, elapsed);
        }

        public override void OnUserOffline(int uid, int elapsed)
        {
            RemoteUserLeftChannelEvent?.Invoke(uid, elapsed);
        }

        public override void OnJoinChannelSuccess(string p0, int p1, int p2)
        {
            JoinedChannelEvent?.Invoke();
        }

        public override void OnLeaveChannel(RtcStats p0)
        {
            LeftChannelEvent?.Invoke();
        }

        [Obsolete]
        public override void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
        }

        public override void OnFirstRemoteVideoFrame(int uid, int width, int height, int elapsed)
        {
            Console.WriteLine($"First remote video decoded");
            FirstVideoDecodedEvent?.Invoke(uid, elapsed);

        }
    }
}
