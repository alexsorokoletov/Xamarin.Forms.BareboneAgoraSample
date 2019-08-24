using System;
using Android.Content;
using FormsBarebone;
using FormsBarebone.Droid;
using Xamarin.Forms;
using DT.Xamarin.Agora;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Views;
using DT.Xamarin.Agora.Video;

[assembly: ExportRenderer(typeof(AgoraVideoCanvas), typeof(AgoraVideoCanvasRenderer))]
[assembly: Dependency(typeof(AgoraEngine))]
namespace FormsBarebone.Droid
{
    public class AgoraVideoCanvasRenderer : ViewRenderer<AgoraVideoCanvas, FrameLayout>
    {

        public AgoraVideoCanvasRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AgoraVideoCanvas> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null) // suggested read - the renderer implementation guide from Xamarin https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view
            {
                if (Control == null)
                {
                    var view = (FrameLayout)LayoutInflater.From(this.Context).Inflate(Resource.Layout.AgoraVideoLayout, null, false);
                    SetNativeControl(view);
                    InitializeAgoraAndroidControl(view, e.NewElement.Uid);
                }
                else
                {
                    InitializeAgoraAndroidControl(Control, e.NewElement.Uid);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(AgoraVideoCanvas.Uid))
            {
                if (Control != null)
                {
                    var uid = Element.Uid;
                    InitializeAgoraAndroidControl(Control, Element.Uid);
                }
            }
        }

        private void InitializeAgoraAndroidControl(FrameLayout container, int uid)
        {
            Console.WriteLine($"Renderer - we should show stream with uid = {uid}");
            if (uid == 0)
            {
                //stop video
            }
            else
            {
                if (container.ChildCount == 0)
                {
                    var surfaceView = RtcEngine.CreateRendererView(DemoHelper.Context);
                    container.AddView(surfaceView);
                    var videoCanvas = new VideoCanvas(surfaceView, Constants.RenderModeAdaptive, uid);
                    var rtcEngine = DemoHelper.CurrentAgoraEngine.RtcEngine;
                    rtcEngine.SetupRemoteVideo(videoCanvas);
                }
            }
        }

        /*
         * see https://docs.agora.io/en/Video/publish_android
        private void setupRemoteVideo(int uid) {
           FrameLayout container = (FrameLayout) findViewById(R.id.remote_video_view_container);

           if (container.getChildCount() >= 1) {
               return;
           }

           SurfaceView surfaceView = RtcEngine.CreateRendererView(getBaseContext());
           container.addView(surfaceView);
           mRtcEngine.setupRemoteVideo(new VideoCanvas(surfaceView, VideoCanvas.RENDER_MODE_ADAPTIVE, uid));

           surfaceView.setTag(uid);
           View tipMsg = findViewById(R.id.quick_tips_when_use_agora_sdk);
           tipMsg.setVisibility(View.GONE);
          }

        */
    }


    public class AgoraEngine : IRtcEngineEventHandler, IAgoraEngine
    {
        private DT.Xamarin.Agora.RtcEngine _rtcEngine;
        internal RtcEngine RtcEngine => _rtcEngine;

        public event Action JoinedChannelEvent;
        public event Action LeftChannelEvent;
        public event RemoteUserJoinedEventType RemoteUserJoinedChannelEvent;
        public event RemoteUserJoinedEventType RemoteUserLeftChannelEvent;

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

    }
}
